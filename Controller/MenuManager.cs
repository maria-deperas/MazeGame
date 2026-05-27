using MazeGame.Characters;
using MazeGame.Commands.Menu;
using MazeGame.Core;
using MazeGame.World;

namespace MazeGame.Commands
{
    public class MenuManager
    {
        private readonly GameModel _model;

        public MenuMode CurrentMenu => _model.CurrentMenu;
        public int SelectionIndex => _model.SelectionIndex;
        public bool IsInMenuMode => _model.IsInMenuMode;
        public Enemy? ActiveEnemy => _model.ActiveEnemy;
        public Cell? ActiveCell => _model.ActiveCell;

        private readonly Dictionary<ConsoleKey, IMenuCommand> _groundActions;
        private readonly Dictionary<ConsoleKey, IMenuCommand> _backpackActions;
        private readonly Dictionary<ConsoleKey, IMenuCommand> _combatActions;

        public MenuManager(GameModel model)
        {
            _model = model;

            _groundActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { Constants.SelectUp, new MoveSelectionUpCommand() },
                { Constants.SelectDown, new MoveSelectionDownCommand() },
                { Constants.PickUp, new PickUpGroundItemCommand() },
                { Constants.CloseMenu, new CloseMenuCommand() }
            };

            _backpackActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { Constants.SelectUp, new MoveSelectionUpCommand() },
                { Constants.SelectDown, new MoveSelectionDownCommand() },
                { Constants.EquipLeft, new EquipLeftCommand() },
                { Constants.EquipRight, new EquipRightCommand() },
                { Constants.UnequipLeft, new UnequipLeftCommand() },
                { Constants.UnequipRight,new UnequipRightCommand() },
                { Constants.DropItem, new DropItemCommand() },
                { Constants.CloseMenu, new CloseMenuCommand() }
            };

            _combatActions = new Dictionary<ConsoleKey, IMenuCommand>
            {
                { Constants.AttackNormal, new NormalAttackCommand() },
                { Constants.AttackStealth, new StealthAttackCommand() },
                { Constants.AttackMagic, new MagicAttackCommand() }
            };
        }

        public void HandleMenuInput(ConsoleKey key)
        {
            if (CurrentMenu == MenuMode.Ground)
            {
                if (!_model.Map.GetCell(_model.Player!.X, _model.Player!.Y).HasItem)
                {
                    _model.CloseMenu();
                    return;
                }
                if (_groundActions.TryGetValue(key, out IMenuCommand? action))
                    action.Execute(_model);
            }
            else if (CurrentMenu == MenuMode.Backpack)
            {
                int cnt = _model.Player!.Backpack.Count;
                if (cnt == 0 && _model.Player!.LeftHand == null && _model.Player!.RightHand == null)
                {
                    _model.CloseMenu();
                    return;
                }
                _model.EnsureValidSelectionIndex(cnt);
                if (_backpackActions.TryGetValue(key, out IMenuCommand? action))
                    action.Execute(_model);
            }
            else if (CurrentMenu == MenuMode.Combat)
            {
                if (_combatActions.TryGetValue(key, out IMenuCommand? action))
                    action.Execute(_model);
            }
        }

        public List<(string Key, string Description)> GetActiveMenuInfo()
        {
            var result = new List<(string, string)>();
            var currentDict = CurrentMenu == MenuMode.Backpack ? _backpackActions :
                              CurrentMenu == MenuMode.Combat   ? _combatActions : _groundActions;
            foreach (var entry in currentDict)
                result.Add((entry.Key.ToString(), entry.Value.Description));
            return result;
        }
    }
}
