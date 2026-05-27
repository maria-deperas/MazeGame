using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class EquipRightCommand : IMenuCommand
    {
        public string Description => "Equip to Right Hand";
        public void Execute(GameModel model) => model.EquipToRight();
    }
}