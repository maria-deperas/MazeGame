using System;
using MazeGame.Characters;
using MazeGame.Items;
using MazeGame.Logging;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class PickUpGroundItemCommand : IMenuCommand
    {
        public string Description => "Pick up";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            Cell currentCell = map.GetCell(player.X, player.Y);

            if (currentCell.Items.Count == 0 || menu.SelectionIndex < 0) return;

            Item selected = currentCell.Items[menu.SelectionIndex];

            if (selected.PickUp(player))
            {
                GameLog.GetInstance().Log($"{player.Name} picks up: {selected.Name}.");
                currentCell.Items.RemoveAt(menu.SelectionIndex);
                menu.CloseMenu();
            }
            else
            {
                showMessage("Backpack is full, you can't pick up " + selected.Name);
            }
        }
    }
}