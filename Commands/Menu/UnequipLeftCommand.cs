using System;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class UnequipLeftCommand : IMenuCommand
    {
        public string Description => "Unequip Left Hand";

        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            if (player.LeftHand != null)
            {
                if (!player.LeftHand.RemoveFromLeft(player))
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
                showMessage("Left hand is already empty!");
            }
        }
    }
}