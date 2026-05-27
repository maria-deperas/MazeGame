using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class MoveSelectionDownCommand : IMenuCommand
    {
        public string Description => "Select Down";
        public void Execute(GameModel model) => model.MoveSelectionDown();
    }
}