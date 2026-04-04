using System;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class CloseMenuCommand : IMenuAction
    {
        public string Description => "Close";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            menu.CloseMenu();
        }
    }
}