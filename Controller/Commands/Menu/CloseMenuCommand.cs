using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public class CloseMenuCommand : IMenuCommand
    {
        public string Description => "Close";
        public void Execute(GameModel model) => model.CloseMenu();
    }
}