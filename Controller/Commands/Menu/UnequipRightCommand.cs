using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class UnequipRightCommand : IMenuCommand
    {
        public string Description => "Unequip Right Hand";
        public void Execute(GameModel model) => model.UnequipRight();
    }
}