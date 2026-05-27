using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class OpenBackpackCommand : ICommand
    {
        private readonly GameModel _model;

        public OpenBackpackCommand(GameModel model)
        {
            _model = model;
        }

        public string Description => "Open backpack";
        public bool IsVisible => _model.Player != null
                                 && (_model.Player.Backpack.Count > 0
                                     || _model.Player.LeftHand != null
                                     || _model.Player.RightHand != null);

        public void Execute() => _model.OpenBackpackMenu();
    }
}