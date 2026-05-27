using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class OpenGroundMenuCommand : ICommand
    {
        private readonly GameModel _model;

        public OpenGroundMenuCommand(GameModel model)
        {
            _model = model;
        }

        public string Description => "Pick up item";
        public bool IsVisible => _model.Player != null && _model.Map.GetCell(_model.Player.X, _model.Player.Y).HasItem;

        public void Execute() => _model.OpenGroundMenu();
    }
}