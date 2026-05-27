using MazeGame.Commands;
using MazeGame.Core;

namespace MazeGame.Network
{
    public class ClientSelectionHandler : BaseInputHandler
    {
        private readonly ClientInputContext _ctx;

        public ClientSelectionHandler(ClientInputContext ctx) => _ctx = ctx;

        public override bool Handle(ConsoleKey key)
        {
            if (key == Constants.SelectUp)   { _ctx.SelectionIndex = Math.Max(0, _ctx.SelectionIndex - 1); return true; }
            if (key == Constants.SelectDown) { _ctx.SelectionIndex++; return true; }
            return base.Handle(key);
        }
    }
}
