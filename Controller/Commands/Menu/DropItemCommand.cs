using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class DropItemCommand : IMenuCommand
    {
        public string Description => "Drop item";
        public void Execute(GameModel model) => model.DropSelectedItem();
    }
}