using MazeGame.Characters;
using MazeGame.World;
using MazeGame.Commands.Menu;
using System.ComponentModel.Design;
using MazeGame.Characters.Combat;
using MazeGame.Logging;

namespace MazeGame.UI
{
    public enum MenuMode { None, Ground, Backpack, Combat }

    public class MenuManager
    {
        public MenuMode CurrentMenu { get; private set; } = MenuMode.None;
        public int SelectionIndex { get; private set; } = -1;
        public bool IsInMenuMode => CurrentMenu != MenuMode.None;
        public Enemy ActiveEnemy { get; private set; }
        public Cell ActiveCell { get; private set; }

        private readonly Dictionary<ConsoleKey, IMenuCommand> _groundActions;
        private readonly Dictionary<ConsoleKey, IMenuCommand> _backpackActions;
        private readonly Dictionary<ConsoleKey, IMenuCommand> _combatActions;

        public MenuManager()
        {
            // Akcje dla mapy
            _groundActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { ConsoleKey.UpArrow, new MoveSelectionUpCommand() },
                { ConsoleKey.DownArrow, new MoveSelectionDownCommand() },
                { ConsoleKey.Enter, new PickUpGroundItemCommand() },
                { ConsoleKey.Escape, new CloseMenuCommand() }
            };

            // Akcje dla plecaka
            _backpackActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { ConsoleKey.UpArrow, new MoveSelectionUpCommand() },
                { ConsoleKey.DownArrow, new MoveSelectionDownCommand() },
                { ConsoleKey.L, new EquipLeftCommand() },
                { ConsoleKey.R, new EquipRightCommand() },
                { ConsoleKey.D1, new UnequipLeftCommand() },
                { ConsoleKey.D2, new UnequipRightCommand() },
                { ConsoleKey.D, new DropItemCommand() },
                { ConsoleKey.Escape, new CloseMenuCommand() }
            };
            // Akcje dla walki
            _combatActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { ConsoleKey.D1, new NormalAttackCommand() },
                { ConsoleKey.D2, new StealthAttackCommand() },
                { ConsoleKey.D3, new MagicAttackCommand() }
            };
        }

        public void ToggleGroundMenu(Player player, Map map, Action<string> showMessage)
        {
            Cell currentCell = map.GetCell(player.X, player.Y);
            if (!currentCell.HasItem) return;

            if (currentCell.Items.Count == 1)
            {
                var item = currentCell.Items[0];
                if (item.PickUp(player))
                {
                    GameLog.GetInstance().Log($"{player.Name} picks up: {item.Name}.");
                    currentCell.Items.RemoveAt(0);
                    map.SoundManager.NotifySubscribers(player.X, player.Y, item.SoundRange, map);
                }
                else showMessage("Backpack is full!");
            }
            else
            {
                CurrentMenu = MenuMode.Ground;
                SelectionIndex = 0;
            }
        }

        public void ToggleBackpackMenu(Player player)
        {
            if (player.Backpack.Count == 0 && player.LeftHand == null && player.RightHand == null) return;
            CurrentMenu = MenuMode.Backpack;
            SelectionIndex = 0;
        }
        public void ToggleCombatMenu(Player player, Enemy enemy, Cell cell, Action<string> showMessage)
        {
            ActiveEnemy = enemy;
            ActiveCell = cell;
            CurrentMenu = MenuMode.Combat;
            SelectionIndex = -1;
            showMessage($"Combat! {enemy.Name} (HP: {enemy.Stats.CurrentHealth})");
        }

        public void HandleMenuInput(Player player, Map map, ConsoleKey key, Action<string> showMessage)
        {
            if (CurrentMenu == MenuMode.Ground)
            {
                Cell currentCell = map.GetCell(player.X, player.Y);
                if (!currentCell.HasItem)
                {
                    CloseMenu();
                    return;
                }

                if (_groundActions.TryGetValue(key, out IMenuCommand action))
                {
                    action.Execute(this, player, map, showMessage);
                }
            }
            else if (CurrentMenu == MenuMode.Backpack)
            {
                int cnt = player.Backpack.Count;

                if (cnt == 0 && player.LeftHand == null && player.RightHand == null)
                {
                    CloseMenu();
                    return;
                }

                if (cnt == 0)
                {
                    SelectionIndex = -1;
                }
                else if (SelectionIndex >= cnt)
                {
                    SelectionIndex = cnt - 1;
                }
                else if (SelectionIndex < 0 && cnt > 0)
                {
                    SelectionIndex = 0;
                }

                if (_backpackActions.TryGetValue(key, out IMenuCommand action))
                {
                    action.Execute(this, player, map, showMessage);
                }
            }
            else if (CurrentMenu == MenuMode.Combat)
            {
                if (_combatActions.TryGetValue(key, out IMenuCommand action))
                {
                    action.Execute(this, player, map, showMessage);
                }
            }
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
            {
                SelectionIndex = -1;
            }
            else if (SelectionIndex >= itemCount)
            {
                SelectionIndex = itemCount - 1;
            }
            else if (SelectionIndex < 0 && itemCount > 0)
            {
                SelectionIndex = 0;
            }
        }

        public List<(string Key, string Description)> GetActiveMenuInfo()
        {
            var result = new List<(string, string)>();

            var currentDict = CurrentMenu == MenuMode.Backpack ? _backpackActions :
                              CurrentMenu == MenuMode.Combat ? _combatActions : _groundActions;

            foreach (var entry in currentDict)
            {
                result.Add((entry.Key.ToString(), entry.Value.Description));
            }
            return result;
        }
    }
}