using System;
using MazeGame.Characters;
using MazeGame.Items;
using MazeGame.Logging;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public class EquipRightCommand : IMenuCommand
    {
        public string Description => "Equip to Right Hand";
        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            if (player.Backpack.Count == 0 || menu.SelectionIndex < 0) return;
            Item selected = player.Backpack[menu.SelectionIndex];

            if (!selected.IsEquippable)
            {
                showMessage("You can only equip weapons!");
            }
            else if (selected.AddToRight(player))
            {
                GameLog.GetInstance().Log($"{player.Name} places {selected.Name} in their right hand.");
                menu.CloseMenu();
            }
            else if (!selected.AddToRight(player))
            {
                showMessage("Right hand can't be emptied!");
            }
        }
    }
}