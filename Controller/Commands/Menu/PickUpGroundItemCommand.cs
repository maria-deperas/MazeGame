using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class PickUpGroundItemCommand : IMenuCommand
    {
        public string Description => "Pick up";
        public void Execute(GameModel model) => model.PickUpSelectedGroundItem();
    }
}