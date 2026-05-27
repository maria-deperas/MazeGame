using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using MazeGame.Characters;
using MazeGame.Characters.Combat;
using MazeGame.Core;
using MazeGame.Logging;
using MazeGame.Network.Dto;
using MazeGame.World;

namespace MazeGame.Network
{
    public class GameServer
    {
        private readonly GameModel _model; // cały stan gry
        private readonly TcpListener _listener; // nasłuchuje na nowe połączenia tcp

        // aktywne sesje - id gracza -> jego sesja tcp
        private readonly ConcurrentDictionary<int, ClientSession> _sessions = new();

        // kolejki do komunikacji między wątkami sieciowymi a główną pętlą gry
        // akcje od klientów
        public readonly ConcurrentQueue<(int id, ActionMessageDto action)> ActionQueue = new();
        // rozłączenia
        public readonly ConcurrentQueue<int> DisconnectedQueue = new();
        // nowe połączenia
        private readonly ConcurrentQueue<(int id, Player player, TcpClient tcp)> _newConnQueue = new();

        // stan walki każdego gracza
        private readonly ConcurrentDictionary<int, (Enemy enemy, Cell cell)> _playerCombats = new();

        private int _nextId = 0;
        private const int MaxPlayers = 9;
        public int ConnectedCount => _sessions.Count;
        public IEnumerable<Enemy> GetCombatEnemies() => _playerCombats.Values.Select(c => c.enemy);

        public GameServer(GameModel model, int port = 5555)
        {
            _model = model;
            _listener = new TcpListener(IPAddress.Any, port); // akceptujemy z każdego interfejsu sieciowego
        }

        // uruchamia nasłuchiwanie
        public void Start()
        {
            _listener.Start();
            _model.Log($"[Server] Nasłuchuję na porcie {((IPEndPoint)_listener.LocalEndpoint).Port}");
            Task.Run(AcceptLoop); // uruchamiamy wątek akceptujący
        }

        // wątek akceptujący - czeka na nowych klientów
        private async Task AcceptLoop()
        {
            while (true)
            {
                try
                {
                    TcpClient tcp = await _listener.AcceptTcpClientAsync(); // czeka na połączenie

                    if (_sessions.Count >= MaxPlayers)
                    {
                        tcp.Close(); // serwer pełny -> odrzucamy połączenie
                        continue;
                    }

                    int id = Interlocked.Increment(ref _nextId); // bezpieczna inkrementacja z wielu wątków jednocześnie
                    var player = new Player(1, 1, $"Player{id}");
                    _newConnQueue.Enqueue((id, player, tcp)); // przekazuje do głównej pętli
                }
                catch { break; }
            }
        }

        // nowe zmiany
        public bool ProcessPendingEvents()
        {
            bool changed = false;

            // nowe połączenia
            while (_newConnQueue.TryDequeue(out var newConn))
            {
                var (id, player, tcp) = newConn;
                var (sx, sy) = FindSpawnPosition(); // szuka wolnego miejsca na mapie
                player.X = sx;
                player.Y = sy;

                var session = new ClientSession(id, player, tcp, this);
                _sessions[id] = session;
                _model.RemotePlayers[id] = player;
                _model.Map.SoundManager?.Subscribe(player); // gracz słyszy dźwięki z mapy
                session.StartAsync(); // startuje ReadLoop i WriteLoop dla każdego klienta

                _model.Log($"[Server] Gracz {id} połączony ({player.Name}). Łącznie: {_sessions.Count}");
                changed = true;
            }

            // obsługa rozłączeń
            while (DisconnectedQueue.TryDequeue(out int disconnId))
            {
                _sessions.TryRemove(disconnId, out _);
                if (_model.RemotePlayers.TryRemove(disconnId, out var disconnPlayer))
                    _model.Map.SoundManager?.Unsubscribe(disconnPlayer); // usuń z subskrybentów dźwięku
                _playerCombats.TryRemove(disconnId, out _);
                _model.Log($"[Server] Gracz {disconnId} rozłączony. Łącznie: {_sessions.Count}");
                changed = true;
            }

            // obsługa akcji
            while (ActionQueue.TryDequeue(out var actionEntry))
            {
                var (playerId, action) = actionEntry;
                ApplyRemoteAction(playerId, action);
                changed = true;
            }

            return changed;
        }

        // tłumaczy akcję od klienta na faktyczną zmianę stanu gry
        private void ApplyRemoteAction(int playerId, ActionMessageDto action)
        {
            if (!_model.RemotePlayers.TryGetValue(playerId, out var player)) return;

            switch (action.Type)
            {
                case ActionType.Move:
                    // jeśli gracz jest w walce, ruch jest zablokowany
                    if (_playerCombats.ContainsKey(playerId)) break;

                    int mx = player.X + action.Dx;
                    int my = player.Y + action.Dy;
                    if (_model.Map.IsMovePossible(mx, my))
                    {
                        Cell tc = _model.Map.GetCell(mx, my);
                        if (tc.HasEnemy)
                        {
                            _playerCombats[playerId] = (tc.Enemy!, tc);
                            _model.Log($"Combat! {player.Name} vs {tc.Enemy!.Name} (HP: {tc.Enemy.Stats.CurrentHealth})");
                            break;
                        }
                    }
                    _model.TryMoveRemotePlayer(player, action.Dx, action.Dy);
                    break;

                case ActionType.Attack:
                    if (!_playerCombats.TryGetValue(playerId, out var combat)) break;

                    // wróg mógł zostać zabity przez kogoś innego
                    if (combat.cell.Enemy != combat.enemy || combat.enemy.Stats.CurrentHealth <= 0)
                    {
                        _playerCombats.TryRemove(playerId, out _);
                        break;
                    }

                    ICombatVisitor visitor = action.AttackType switch
                    {
                        AttackKind.Stealth => new StealthAttack(),
                        AttackKind.Magic   => new MagicAttack(),
                        AttackKind.Normal  => new NormalAttack()
                    };
                    
                    bool enemyDied = _model.ExecuteAttackForPlayer(visitor, player, combat.enemy, combat.cell);
                    if (enemyDied || player.Stats.Health <= 0)
                    {
                        _playerCombats.TryRemove(playerId, out _);
                        if (player.Stats.Health <= 0)
                            _model.Log($"[Server] {player.Name} zginął!");
                    }
                    break;

                case ActionType.PickUp:
                    if (_playerCombats.ContainsKey(playerId)) break;
                    int pickIdx = action.SelectionIndex >= 0 ? action.SelectionIndex : 0;
                    _model.PickUpItemForPlayer(player, pickIdx);
                    break;

                case ActionType.EquipLeft:
                    _model.EquipToLeftForPlayer(player, action.SelectionIndex);
                    break;

                case ActionType.EquipRight:
                    _model.EquipToRightForPlayer(player, action.SelectionIndex);
                    break;

                case ActionType.UnequipLeft:
                    _model.UnequipLeftForPlayer(player);
                    break;

                case ActionType.UnequipRight:
                    _model.UnequipRightForPlayer(player);
                    break;

                case ActionType.DropItem:
                    _model.DropItemForPlayer(player, action.SelectionIndex);
                    break;

                case ActionType.CloseMenu:
                    _playerCombats.TryRemove(playerId, out _);
                    break;
            }
        }
        
        // wysyła aktualny stan gry do wszystkich klientów
        public void Broadcast()
        {
            if (_sessions.IsEmpty) return;
 
            // wszyscy gracze
            var allPlayers = new List<(int id, Player player)>();
            if (_model.Player != null) allPlayers.Add((0, _model.Player));
            allPlayers.AddRange(_model.RemotePlayers.Select(kv => (kv.Key, kv.Value)));

            // serializacja mapy i graczy
            var grid = GameStateMapper.MapGrid(_model.Map);
            var playerDtos = allPlayers.Select(p => GameStateMapper.ToDto(p.player, p.id)).ToList();

            var recentLogs = GameLog.GetInstance().Memory?.GetRecentLogs(4) ?? new List<string>();

            // każdy klient dostaje swój własny dto
            foreach (var (id, session) in _sessions)
            {
                EnemyDto? activeEnemy = null;
                if (_playerCombats.TryGetValue(id, out var combat))
                    activeEnemy = GameStateMapper.ToDto(combat.enemy);

                var dto = new GameStateDto
                {
                    Grid          = grid,
                    Players       = playerDtos,
                    LocalPlayerId = id,
                    ActiveEnemy   = activeEnemy,
                    RecentLogs    = recentLogs
                };
                session.Send(JsonProtocol.EncodeState(dto));
            }
        }

        // wolna pozycja dla nowego gracza
        private (int x, int y) FindSpawnPosition()
        {
            var rnd = new Random();
            for (int attempt = 0; attempt < 200; attempt++)
            {
                int x = rnd.Next(1, Constants.MazeWidth  - 1);
                int y = rnd.Next(1, Constants.MazeHeight - 1);
                if (_model.Map.IsMovePossible(x, y) && !IsOccupied(x, y))
                    return (x, y);
            }
            return (1, 1);
        }

        private bool IsOccupied(int x, int y)
        {
            if (_model.Player != null && _model.Player.X == x && _model.Player.Y == y) return true;
            return _model.RemotePlayers.Values.Any(p => p.X == x && p.Y == y);
        }
    }
}
