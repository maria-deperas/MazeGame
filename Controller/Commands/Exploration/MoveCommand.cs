using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class MoveCommand : ICommand
    {
        private readonly GameModel _model;
        private readonly int _dx;
        private readonly int _dy;

        public MoveCommand(GameModel model, int dx, int dy)
        {
            _model = model;
            _dx = dx;
            _dy = dy;
        }

        public string Description => "Move";
        public bool IsVisible => false;

        public void Execute() => _model.TryMove(_dx, _dy);
    }
}