using MazeGame.Core;
using MazeGame.Network;
using MazeGame.Network.Dto;
using MazeGame.UI.Panels;

namespace MazeGame.UI
{
    public class NetworkGameView : IGameView
    {
        private volatile GameStateDto? _state;
        private int _localPlayerId;

        private readonly ClientInputContext _ctx;
        private readonly ScreenRenderer _renderer;
        private readonly IRenderable _leftPanel;
        private readonly IRenderable _middlePanel;
        private readonly IRenderable _rightPanel;

        public NetworkGameView(ClientInputContext ctx, Func<List<string>> getControlsInstructions)
        {
            _ctx = ctx;
            _renderer = new ScreenRenderer();

            PlayerDto? LocalPlayer() =>
                _state?.Players.FirstOrDefault(p => p.PlayerId == _localPlayerId);

            List<ItemDto>? GroundItems()
            {
                var p = LocalPlayer();
                if (p == null || _state == null) return null;
                var grid = _state.Grid;
                if (p.Y >= grid.Count || p.X >= grid[p.Y].Count) return null;
                return grid[p.Y][p.X].Items;
            }

            _leftPanel = new VerticalStackPanel(
                Constants.LeftPanelWidth,
                Constants.PanelHeight,
                new ClientAttributesPanel(LocalPlayer),
                new ClientWalletPanel(LocalPlayer),
                new ClientControlsPanel(getControlsInstructions)
            );

            _middlePanel = new ClientMapRenderer(
                () => _state,
                () => _localPlayerId
            );

            _rightPanel = new VerticalStackPanel(
                Constants.RightPanelWidth,
                Constants.PanelHeight,
                new ClientEquippedPanel(LocalPlayer),
                new ClientGroundPanel(GroundItems, () => _ctx.SelectionIndex),
                new ClientBackpackPanel(LocalPlayer, () => _ctx.SelectionIndex),
                new ClientCombatPanel(() => _state?.ActiveEnemy),
                new ClientPlayersPanel(() => _state?.Players, () => _localPlayerId),
                new ClientLogPanel(() => _state?.RecentLogs)
            );
        }

        public void UpdateState(GameStateDto state)
        {
            if (_localPlayerId == 0 && state.LocalPlayerId != 0)
                _localPlayerId = state.LocalPlayerId;
            _state = state;
        }

        public void Render() => _renderer.DrawScreen(_leftPanel, _middlePanel, _rightPanel);

        public void ShowConnectionError(string host, int port, string message)
        {
            Console.WriteLine($"[Klient] Nie można połączyć się z {host}:{port} — {message}");
        }

        public void ShowConnecting()
        {
            Console.WriteLine("[Klient] Połączono. Oczekuję na dane z serwera...");
        }

        public void ShowDisconnected()
        {
            Console.Clear();
            Console.WriteLine("\n  Rozłączono z serwera. Naciśnij dowolny klawisz.");
        }

        public void ShowGameOver()
        {
            Console.Clear();
            Console.WriteLine(@"
               ___   _   __  __ ___    _____   _____ ___
              / __| /_\ |  \/  | __|  / _ \ \ / / __| _ \
             | (_ |/ _ \| |\/| | _|  | (_) \ V /| _||   /
              \___/_/ \_\_|  |_|___|  \___/ \_/ |___|_|_\
            ");
            Console.WriteLine("\n  Naciśnij dowolny klawisz...");
            Console.ReadKey(true);
        }
    }
}
