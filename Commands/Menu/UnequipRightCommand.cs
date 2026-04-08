using System;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class UnequipRightCommand : IMenuCommand
    {
        public string Description => "Unequip Right Hand";

        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            if (player.RightHand != null)
            {
                if (!player.RightHand.RemoveFromRight(player))
                {
                    showMessage("Not enough space in backpack!");
                }
                else
                {
                    if (player.Backpack.Count == 1)
                        menu.EnsureValidSelectionIndex(0);
                }
            }
            else
            {
                showMessage("Right hand is already empty!");
            }
        }
    }
}