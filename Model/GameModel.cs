using System.Collections.Concurrent;
using MazeGame.Characters;
using MazeGame.Characters.Combat;
using MazeGame.Logging;
using MazeGame.World;
using MazeGame.Items;

namespace MazeGame.Core
{
    public enum MenuMode { None, Ground, Backpack, Combat }

    // centralny model gry
    // przechowuje stan gry i udostępnia metody do jego zmiany
    // nie wie nic o wyświetlaniu ani sterowaniu
    public class GameModel
    {
        public Map Map { get; }
        public Player? Player { get; }

        public MenuMode CurrentMenu { get; private set; } = MenuMode.None;
        public int SelectionIndex { get; private set; } = -1;
        public bool IsInMenuMode => CurrentMenu != MenuMode.None;
        public Enemy? ActiveEnemy { get; private set; }
        public Cell? ActiveCell { get; private set; }
        public ConcurrentDictionary<int, Player> RemotePlayers { get; } = new();
        private readonly GameLog _log;
        private DateTime _lastEnemyTick = DateTime.UtcNow;
        private static readonly TimeSpan EnemyTickInterval = TimeSpan.FromSeconds(2);

        public GameModel(Map map, Player? player)
        {
            Map = map;
            Player = player;
            _log = GameLog.GetInstance();

            // gracz usłyszy dźwięki wydawane przez innych graczy
            if (Player != null)
                Map.SoundManager?.Subscribe(Player);
        }

        // Ruch
        public void TryMove(int dx, int dy)
        {
            var (nextX, nextY) = Player!.GetNextPosition(dx, dy);

            if (!Map.IsMovePossible(nextX, nextY))
            {
                _log.Log($"{Player!.Name} hits the wall.");
                return;
            }

            Cell targetCell = Map.GetCell(nextX, nextY);
            if (targetCell.HasEnemy)
            {
                OpenCombatMenu(targetCell.Enemy!, targetCell);
            }
            else
            {
                Player!.Move(nextX, nextY);
            }
        }

        public bool TryTickEnemies(IEnumerable<Enemy>? extraLocked = null)
        {
            if (DateTime.UtcNow - _lastEnemyTick < EnemyTickInterval) return false;
            _lastEnemyTick = DateTime.UtcNow;

            IEnumerable<Enemy>? locked = null;
            if (ActiveEnemy != null || extraLocked != null)
            {
                var list = new List<Enemy>();
                if (ActiveEnemy != null) list.Add(ActiveEnemy);
                if (extraLocked != null) list.AddRange(extraLocked);
                locked = list;
            }

            // zbieramy pozycje wszystkich graczy
            var allPlayerPos = new HashSet<(int x, int y)>();
            if (Player != null) allPlayerPos.Add((Player.X, Player.Y));
            foreach (var rp in RemotePlayers.Values) allPlayerPos.Add((rp.X, rp.Y));
            Map.MoveEnemies(allPlayerPos.Count > 0 ? allPlayerPos : null, locked);
            return true;
        }

        // Walka

        public void ExecuteAttack(ICombatVisitor visitor)
        {
            Enemy? enemy = ActiveEnemy;
            if (enemy == null) return;

            int playerDamage = 0;
            int playerDefense = 0;
            bool holdsAnyWeapon = false;

            if (Player!.RightHand != null)
            {
                playerDamage += Player!.RightHand.AcceptDamage(visitor, Player!.Stats);
                playerDefense = Player!.RightHand.AcceptDefense(visitor, Player!.Stats);
                holdsAnyWeapon = true;
            }
            if (Player!.LeftHand != null)
            {
                playerDamage += Player!.LeftHand.AcceptDamage(visitor, Player!.Stats);
                playerDefense = Player!.LeftHand.AcceptDefense(visitor, Player!.Stats);
                holdsAnyWeapon = true;
            }
            if (!holdsAnyWeapon)
            {
                playerDamage = visitor.CalculateDamageNonWeapon();
                playerDefense = visitor.CalculateDefenseNonWeapon(Player!.Stats);
            }

            int actualDamageToEnemy = Math.Max(0, playerDamage - enemy.Stats.Armor);
            enemy.Stats.CurrentHealth -= actualDamageToEnemy;
            _log.Log($"Hit {enemy.Name} for {actualDamageToEnemy} dmg!");

            if (enemy.Stats.CurrentHealth <= 0)
            {
                enemy.Die();
                _log.Log($"{enemy.Name} died!");
                if (ActiveCell != null) ActiveCell.Enemy = null;
                CloseMenu();
                return;
            }

            int actualDamageToPlayer = Math.Max(0, enemy.Stats.Attack - playerDefense);
            Player!.Stats.Health -= actualDamageToPlayer;
            _log.Log($"{enemy.Name} counter-attacks for {actualDamageToPlayer} dmg!");

            if (Player!.Stats.Health <= 0)
            {
                _log.Log("YOU DIED! GAME OVER.");
                CloseMenu();
            }
        }

        // Stan menu

        public void OpenGroundMenu()
        {
            Cell currentCell = Map.GetCell(Player!.X, Player!.Y);
            if (!currentCell.HasItem) return;

            if (currentCell.Items.Count == 1)
            {
                var item = currentCell.Items[0];
                if (item.PickUp(Player))
                {
                    _log.Log($"{Player!.Name} picks up: {item.Name}.");
                    currentCell.Items.RemoveAt(0);
                    Map.SoundManager?.NotifySubscribers(Player!.X, Player!.Y, item.SoundRange, Map);
                }
                else
                {
                    _log.Log("Backpack is full!");
                }
            }
            else
            {
                CurrentMenu = MenuMode.Ground;
                SelectionIndex = 0;
            }
        }

        public void OpenBackpackMenu()
        {
            if (Player!.Backpack.Count == 0 && Player!.LeftHand == null && Player!.RightHand == null) return;
            CurrentMenu = MenuMode.Backpack;
            SelectionIndex = 0;
        }

        public void OpenCombatMenu(Enemy enemy, Cell cell)
        {
            ActiveEnemy = enemy;
            ActiveCell = cell;
            CurrentMenu = MenuMode.Combat;
            SelectionIndex = -1;
            _log.Log($"Combat! {enemy.Name} (HP: {enemy.Stats.CurrentHealth})");
        }

        public void CloseMenu()
        {
            CurrentMenu = MenuMode.None;
            SelectionIndex = -1;
            ActiveEnemy = null;
            ActiveCell = null;
        }

        public void MoveSelection(int step, int itemCount)
        {
            SelectionIndex = (SelectionIndex + step + itemCount) % itemCount;
        }

        public void EnsureValidSelectionIndex(int itemCount)
        {
            if (itemCount == 0)
                SelectionIndex = -1;
            else if (SelectionIndex >= itemCount)
                SelectionIndex = itemCount - 1;
            else if (SelectionIndex < 0 && itemCount > 0)
                SelectionIndex = 0;
        }
        
        public void MoveSelectionUp()
        {
            int cnt = CurrentMenu == MenuMode.Ground
                ? Map.GetCell(Player!.X, Player!.Y).Items.Count
                : Player!.Backpack.Count;
            if (cnt > 0) MoveSelection(-1, cnt);
        }

        public void MoveSelectionDown()
        {
            int cnt = CurrentMenu == MenuMode.Ground
                ? Map.GetCell(Player!.X, Player!.Y).Items.Count
                : Player!.Backpack.Count;
            if (cnt > 0) MoveSelection(1, cnt);
        }

        // Ekwipunek 

        public void EquipToLeft()
        {
            if (Player!.Backpack.Count == 0 || SelectionIndex < 0) return;
            Item selected = Player!.Backpack[SelectionIndex];
            if (!selected.IsEquippable)
            {
                _log.Log("You can only equip weapons or unusable items!");
            }
            else if (selected.AddToLeft(Player))
            {
                _log.Log($"{Player!.Name} places {selected.Name} in their left hand.");
                CloseMenu();
            }
            else
            {
                _log.Log("Left hand can't be emptied!");
            }
        }

        public void EquipToRight()
        {
            if (Player!.Backpack.Count == 0 || SelectionIndex < 0) return;
            Item selected = Player!.Backpack[SelectionIndex];
            if (!selected.IsEquippable)
            {
                _log.Log("You can only equip weapons!");
            }
            else if (selected.AddToRight(Player))
            {
                _log.Log($"{Player!.Name} places {selected.Name} in their right hand.");
                CloseMenu();
            }
            else
            {
                _log.Log("Right hand can't be emptied!");
            }
        }

        public void UnequipLeft()
        {
            if (Player!.LeftHand == null) { _log.Log("Left hand is already empty!"); return; }
            if (!Player!.LeftHand.RemoveFromLeft(Player))
                _log.Log("Not enough space in backpack!");
            else if (Player!.Backpack.Count == 1)
                EnsureValidSelectionIndex(0);
        }

        public void UnequipRight()
        {
            if (Player!.RightHand == null) { _log.Log("Right hand is already empty!"); return; }
            if (!Player!.RightHand.RemoveFromRight(Player))
                _log.Log("Not enough space in backpack!");
            else if (Player!.Backpack.Count == 1)
                EnsureValidSelectionIndex(0);
        }

        public void DropSelectedItem()
        {
            if (SelectionIndex < 0 || SelectionIndex >= Player!.Backpack.Count) return;
            Item selected = Player!.Backpack[SelectionIndex];
            int cnt = Player!.Backpack.Count;
            if (selected.Drop(Player, Map.GetCell(Player!.X, Player!.Y)))
            {
                if (cnt - 1 == 0) CloseMenu();
                else EnsureValidSelectionIndex(cnt - 1);
            }
            else
            {
                _log.Log("Too many items on this tile!");
            }
        }

        public void PickUpSelectedGroundItem()
        {
            Cell currentCell = Map.GetCell(Player!.X, Player!.Y);
            if (currentCell.Items.Count == 0 || SelectionIndex < 0) return;
            Item selected = currentCell.Items[SelectionIndex];
            if (selected.PickUp(Player))
            {
                _log.Log($"{Player!.Name} picks up: {selected.Name}.");
                currentCell.Items.RemoveAt(SelectionIndex);
                Map.SoundManager?.NotifySubscribers(Player!.X, Player!.Y, selected.SoundRange, Map);
                CloseMenu();
            }
            else
            {
                _log.Log($"Backpack is full, you can't pick up {selected.Name}");
            }
        }

        // ruch gracza zdalnego, wywoływana z wątku głównego serwera
        public void TryMoveRemotePlayer(Player player, int dx, int dy)
        {
            var (nextX, nextY) = player.GetNextPosition(dx, dy);

            if (!Map.IsMovePossible(nextX, nextY)) { _log.Log($"{player.Name} hits the wall."); return; }

            if (Player != null && Player!.X == nextX && Player!.Y == nextY) return;

            // Kolizja z innym graczem
            if (RemotePlayers.Values.Any(p => p != player && p.X == nextX && p.Y == nextY)) return;

            Cell targetCell = Map.GetCell(nextX, nextY);
            if (targetCell.HasEnemy)
            {
                Enemy enemy = targetCell.Enemy!;
                int damage = Math.Max(0, player.Stats.Strength / 2 - enemy.Stats.Armor);
                enemy.Stats.CurrentHealth -= damage;
                _log.Log($"{player.Name} hits {enemy.Name} for {damage}!");

                if (enemy.Stats.CurrentHealth <= 0)
                {
                    enemy.Die();
                    _log.Log($"{enemy.Name} died!");
                    targetCell.Enemy = null;
                }
                else
                {
                    int counter = Math.Max(0, enemy.Stats.Attack);
                    player.Stats.Health -= counter;
                    _log.Log($"{enemy.Name} counter-attacks {player.Name} for {counter}!");
                }
            }
            else
            {
                player.Move(nextX, nextY);
            }
        }
        
        public void PickUpItemForPlayer(Player player, int index = 0)
        {
            Cell cell = Map.GetCell(player.X, player.Y);
            if (!cell.HasItem || index < 0 || index >= cell.Items.Count) return;

            Item item = cell.Items[index];
            if (item.PickUp(player))
            {
                _log.Log($"{player.Name} picks up: {item.Name}.");
                cell.Items.RemoveAt(index);
                Map.SoundManager?.NotifySubscribers(player.X, player.Y, item.SoundRange, Map);
            }
            else
            {
                _log.Log($"{player.Name}: backpack is full!");
            }
        }
        
        public void EquipToLeftForPlayer(Player player, int index)
        {
            if (index < 0 || index >= player.Backpack.Count) return;
            Item item = player.Backpack[index];
            if (item.IsEquippable && item.AddToLeft(player))
                _log.Log($"{player.Name} places {item.Name} in their left hand.");
            else
                _log.Log($"{player.Name}: can't equip {item.Name} to left hand.");
        }

        public void EquipToRightForPlayer(Player player, int index)
        {
            if (index < 0 || index >= player.Backpack.Count) return;
            Item item = player.Backpack[index];
            if (item.IsEquippable && item.AddToRight(player))
                _log.Log($"{player.Name} places {item.Name} in their right hand.");
            else
                _log.Log($"{player.Name}: can't equip {item.Name} to right hand.");
        }

        public void UnequipLeftForPlayer(Player player)
        {
            if (player.LeftHand == null) return;
            if (!player.LeftHand.RemoveFromLeft(player))
                _log.Log($"{player.Name}: not enough backpack space!");
        }

        public void UnequipRightForPlayer(Player player)
        {
            if (player.RightHand == null) return;
            if (!player.RightHand.RemoveFromRight(player))
                _log.Log($"{player.Name}: not enough backpack space!");
        }

        public void DropItemForPlayer(Player player, int index)
        {
            if (index < 0 || index >= player.Backpack.Count) return;
            Item item = player.Backpack[index];
            Cell cell = Map.GetCell(player.X, player.Y);
            if (!item.Drop(player, cell))
                _log.Log($"{player.Name}: too many items on this tile!");
        }

        // zwraca true jeśli wróg zginął
        public bool ExecuteAttackForPlayer(ICombatVisitor visitor, Player player, Enemy enemy, Cell cell)
        {
            int playerDamage  = 0;
            int playerDefense = 0;
            bool holdsWeapon  = false;

            if (player.RightHand != null)
            {
                playerDamage  += player.RightHand.AcceptDamage(visitor, player.Stats);
                playerDefense  = player.RightHand.AcceptDefense(visitor, player.Stats);
                holdsWeapon    = true;
            }
            if (player.LeftHand != null)
            {
                playerDamage  += player.LeftHand.AcceptDamage(visitor, player.Stats);
                playerDefense  = player.LeftHand.AcceptDefense(visitor, player.Stats);
                holdsWeapon    = true;
            }
            if (!holdsWeapon)
            {
                playerDamage  = visitor.CalculateDamageNonWeapon();
                playerDefense = visitor.CalculateDefenseNonWeapon(player.Stats);
            }

            int dmgToEnemy = Math.Max(0, playerDamage - enemy.Stats.Armor);
            enemy.Stats.CurrentHealth -= dmgToEnemy;
            _log.Log($"{player.Name} hits {enemy.Name} for {dmgToEnemy} dmg!");

            if (enemy.Stats.CurrentHealth <= 0)
            {
                enemy.Die();
                _log.Log($"{enemy.Name} died!");
                cell.Enemy = null;
                return true;
            }

            int dmgToPlayer = Math.Max(0, enemy.Stats.Attack - playerDefense);
            player.Stats.Health -= dmgToPlayer;
            _log.Log($"{enemy.Name} counter-attacks {player.Name} for {dmgToPlayer} dmg!");
            return false;
        }

        public void Log(string message) => _log.Log(message);
    }
}