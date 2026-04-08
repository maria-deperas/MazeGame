using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class MoveSelectionUpCommand : IMenuCommand
    {
        public string Description => "Select Up";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            int cnt = menu.CurrentMenu == MenuMode.Ground
                ? map.GetCell(player.X, player.Y).Items.Count
                : player.Backpack.Count;

            menu.MoveSelection(-1, cnt);
        }
    }
}