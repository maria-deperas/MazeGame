using MazeGame.Characters;
using MazeGame.World;
using MazeGame.Commands.Menu;
using System.ComponentModel.Design;
using MazeGame.Characters.Combat;

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

        // Słowniki zastępujące instrukcje switch!
        private readonly Dictionary<ConsoleKey, IMenuAction> _groundActions;
        private readonly Dictionary<ConsoleKey, IMenuAction> _backpackActions;
        private readonly Dictionary<ConsoleKey, IMenuAction> _combatActions;

        public MenuManager()
        {
            // Rejestrujemy akcje dla menu na ziemi
            _groundActions = new Dictionary<ConsoleKey, IMenuAction>
            {
                { ConsoleKey.UpArrow, new MoveSelectionUpAction() },
                { ConsoleKey.DownArrow, new MoveSelectionDownAction() },
                { ConsoleKey.Enter, new PickUpGroundItemAction() },
                { ConsoleKey.Escape, new CloseMenuCommand() }
            };

            // Rejestrujemy akcje dla menu plecaka
            _backpackActions = new Dictionary<ConsoleKey, IMenuAction>
            {
                { ConsoleKey.UpArrow, new MoveSelectionUpAction() },
                { ConsoleKey.DownArrow, new MoveSelectionDownAction() },
                { ConsoleKey.L, new EquipLeftCommand() },
                { ConsoleKey.R, new EquipRightAction() },
                { ConsoleKey.D1, new UnequipLeftAction() },
                { ConsoleKey.D2, new UnequipRightAction() },
                { ConsoleKey.D, new DropItemCommand() },
                { ConsoleKey.Escape, new CloseMenuCommand() }
            };
            _combatActions = new Dictionary<ConsoleKey, IMenuAction>
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
                if (currentCell.Items[0].PickUp(player))
                {
                    currentCell.Items.RemoveAt(0);
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
            showMessage($"Combat! {enemy.Name} (HP: {enemy.Stats.CurrentHealth}). Choose attack: [1] Normal, [2] Stealth, [3] Magic");
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

                if (_groundActions.TryGetValue(key, out IMenuAction action))
                {
                    action.Execute(this, player, map, showMessage);
                }
            }
            else if (CurrentMenu == MenuMode.Backpack)
            {
                int cnt = player.Backpack.Count;

                // ZMIANA 1: Zamykamy plecak tylko, gdy CAŁKOWICIE nie ma nic do zarządzania
                if (cnt == 0 && player.LeftHand == null && player.RightHand == null)
                {
                    CloseMenu();
                    return;
                }

                // ZMIANA 2: Zabezpieczenie kursora, gdy plecak jest pusty
                if (cnt == 0)
                {
                    SelectionIndex = -1; // Ukrywamy kursor, bo nie ma na czym go pokazać
                }
                else if (SelectionIndex >= cnt)
                {
                    SelectionIndex = cnt - 1;
                }
                else if (SelectionIndex < 0 && cnt > 0)
                {
                    SelectionIndex = 0; // Ustawiamy na 1. element, jeśli coś się pojawiło
                }

                // Uruchamiamy akcję (teraz wciśnięcie klawisza 1 w końcu tu dotrze!)
                if (_backpackActions.TryGetValue(key, out IMenuAction action))
                {
                    action.Execute(this, player, map, showMessage);
                }
            }
            else if (CurrentMenu == MenuMode.Combat)
            {
                if (_combatActions.TryGetValue(key, out IMenuAction action))
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
            if (SelectionIndex >= itemCount)
                SelectionIndex = Math.Max(0, itemCount - 1);
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