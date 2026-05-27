using MazeGame.Commands;
using MazeGame.Core;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    public class ClientMovementHandler : BaseInputHandler
    {
        private readonly ClientInputContext _ctx;
        private readonly Dictionary<ConsoleKey, (int Dx, int Dy)> _bindings = new()
        {
            { Constants.MoveUp,    (0, -1) },
            { Constants.MoveDown,  (0,  1) },
            { Constants.MoveLeft,  (-1, 0) },
            { Constants.MoveRight, ( 1, 0) }
        };

        public ClientMovementHandler(ClientInputContext ctx) => _ctx = ctx;

        public override bool Handle(ConsoleKey key)
        {
            if (_bindings.TryGetValue(key, out var dir))
            {
                _ctx.PendingAction = new ActionMessageDto { Type = ActionType.Move, Dx = dir.Dx, Dy = dir.Dy };
                return true;
            }
            return base.Handle(key);
        }

        public override List<string> GetGuiInstructions()
        {
            string keys = string.Join("/", _bindings.Keys.Select(FormatKey));
            return new List<string> { $" [{keys}] Move" };
        }
    }
}
