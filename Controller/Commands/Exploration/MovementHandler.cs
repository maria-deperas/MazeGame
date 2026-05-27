using MazeGame.Commands;
using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class MovementHandler : BaseInputHandler
    {
        private readonly Dictionary<ConsoleKey, ICommand> _keyBindings;

        public MovementHandler(GameModel model)
        {
            _keyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { Constants.MoveUp,    new MoveCommand(model, 0, -1) },
                { Constants.MoveDown,  new MoveCommand(model, 0,  1) },
                { Constants.MoveLeft,  new MoveCommand(model, -1, 0) },
                { Constants.MoveRight, new MoveCommand(model,  1, 0) }
            };
        }

        public override bool Handle(ConsoleKey key)
        {
            if (_keyBindings.TryGetValue(key, out ICommand? command))
            {
                command.Execute();
                return true;
            }
            return base.Handle(key);
        }

        public override List<string> GetGuiInstructions()
        {
            var keys = string.Join("/", _keyBindings.Keys);
            return new List<string> { $" [{keys}] Move" };
        }
    }
}