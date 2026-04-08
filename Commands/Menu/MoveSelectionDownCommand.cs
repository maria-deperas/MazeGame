using System;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class MoveSelectionDownCommand : IMenuCommand
    {
        public string Description => "Select Down";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            int cnt = menu.CurrentMenu == MenuMode.Ground
                ? map.GetCell(player.X, player.Y).Items.Count
                : player.Backpack.Count;

            menu.MoveSelection(1, cnt);
        }
    }
}