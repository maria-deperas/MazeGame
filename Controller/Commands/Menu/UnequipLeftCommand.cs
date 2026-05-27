using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class UnequipLeftCommand : IMenuCommand
    {
        public string Description => "Unequip Left Hand";
        public void Execute(GameModel model) => model.UnequipLeft();
    }
}