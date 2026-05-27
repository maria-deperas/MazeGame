using MazeGame.Commands;
using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class MenuInputHandler : BaseInputHandler
    {
        private readonly MenuManager _menuManager;

        public MenuInputHandler(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public override bool Handle(ConsoleKey key)
        {
            if (_menuManager.IsInMenuMode)
            {
                _menuManager.HandleMenuInput(key);
                return true;
            }
            return base.Handle(key);
        }
    }
}