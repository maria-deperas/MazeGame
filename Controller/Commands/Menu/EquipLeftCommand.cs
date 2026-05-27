using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class EquipLeftCommand : IMenuCommand
    {
        public string Description => "Equip to Left Hand";
        public void Execute(GameModel model) => model.EquipToLeft();
    }
}