using System;
using MazeGame.Characters;
using MazeGame.Items;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class DropItemCommand : IMenuAction
    {
        public string Description => "Drop item";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            Item selected = player.Backpack[menu.SelectionIndex];
            int cnt = player.Backpack.Count;

            if (selected.Drop(player, map.GetCell(player.X, player.Y)))
            {
                if (cnt - 1 == 0) menu.CloseMenu();
                else menu.EnsureValidSelectionIndex(cnt - 1);
            }
            else showMessage("Too many items on this tile!");
        }
    }
}