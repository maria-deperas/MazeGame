using MazeGame.Commands;
using MazeGame.Core;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    public class ClientBackpackHandler : BaseInputHandler
    {
        private readonly ClientInputContext _ctx;

        private readonly Dictionary<ConsoleKey, IClientMenuCommand> _backpackActions;

        public ClientBackpackHandler(ClientInputContext ctx)
        {
            _ctx = ctx;
            _backpackActions = new Dictionary<ConsoleKey, IClientMenuCommand>
            {
                { Constants.EquipLeft, new ClientEquipLeftCommand()},
                { Constants.EquipRight, new ClientEquipRightCommand()},
                { Constants.UnequipLeft, new ClientUnequipLeftCommand()},
                { Constants.UnequipRight, new ClientUnequipRightCommand()},
                { Constants.DropItem, new ClientDropItemCommand()},
                { Constants.CloseMenu, new ClientCloseBackpackCommand()}
            };
        }

        public override bool Handle(ConsoleKey key)
        {
            if (key == Constants.OpenBackpack)
            {
                if (!_ctx.IsInCombat)
                    _ctx.IsInBackpackMode = !_ctx.IsInBackpackMode;
                return true;
            }

            if (_ctx.IsInBackpackMode && !_ctx.IsInCombat)
            {
                if (_backpackActions.TryGetValue(key, out var cmd))
                {
                    cmd.Execute(_ctx);
                    return true;
                }
            }

            return base.Handle(key);
        }

        public override List<string> GetGuiInstructions()
        {
            if (_ctx.IsInBackpackMode)
                return _backpackActions
                    .Select(kv => $" [{FormatKey(kv.Key)}] {kv.Value.Description}")
                    .ToList();

            return new List<string> { $" [{FormatKey(Constants.OpenBackpack)}] Backpack" };
        }
    }
}
