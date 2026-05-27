using MazeGame.Commands;
using MazeGame.Core;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    public class ClientPickUpHandler : BaseInputHandler
    {
        private readonly ClientInputContext _ctx;

        public ClientPickUpHandler(ClientInputContext ctx) => _ctx = ctx;

        public override bool Handle(ConsoleKey key)
        {
            if (key == Constants.OpenGround || key == Constants.PickUp)
            {
                _ctx.PendingAction = new ActionMessageDto { Type = ActionType.PickUp, SelectionIndex = _ctx.SelectionIndex };
                return true;
            }
            return base.Handle(key);
        }

        public override List<string> GetGuiInstructions()
            => new List<string>
            {
                $" [{FormatKey(Constants.OpenGround)}/{FormatKey(Constants.PickUp)}] Pick up"
            };
    }
}
