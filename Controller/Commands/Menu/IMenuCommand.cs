using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public interface IMenuCommand
    {
        string Description { get; }
        void Execute(GameModel model);
    }
}