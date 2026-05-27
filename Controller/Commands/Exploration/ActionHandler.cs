using MazeGame.Commands;
using MazeGame.Core;

namespace MazeGame.Commands.Exploration
{
    public class ActionHandler : BaseInputHandler
    {
        private readonly Dictionary<ConsoleKey, ICommand> _keyBindings;

        public ActionHandler(GameModel model, Action showLog)
        {
            _keyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { Constants.OpenGround,   new OpenGroundMenuCommand(model) },
                { Constants.OpenBackpack, new OpenBackpackCommand(model) },
                { Constants.OpenLog,      new OpenLogCommand(showLog) }
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
            var instructions = new List<string>();
            foreach (var binding in _keyBindings)
            {
                if (binding.Value.IsVisible)
                    instructions.Add($" [{binding.Key}] {binding.Value.Description}");
            }
            return instructions;
        }
    }
}