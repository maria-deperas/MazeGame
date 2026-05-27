using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class MoveSelectionUpCommand : IMenuCommand
    {
        public string Description => "Select Up";
        public void Execute(GameModel model) => model.MoveSelectionUp();
    }
}