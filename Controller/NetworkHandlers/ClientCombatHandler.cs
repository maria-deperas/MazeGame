using MazeGame.Commands;
using MazeGame.Core;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    public class ClientCombatHandler : BaseInputHandler
    {
        private readonly ClientInputContext _ctx;
        private readonly Dictionary<ConsoleKey, AttackKind> _attacks = new()
        {
            { Constants.AttackNormal,  AttackKind.Normal  },
            { Constants.AttackStealth, AttackKind.Stealth },
            { Constants.AttackMagic,   AttackKind.Magic   }
        };

        public ClientCombatHandler(ClientInputContext ctx) => _ctx = ctx;

        public override bool Handle(ConsoleKey key)
        {
            if (_attacks.TryGetValue(key, out AttackKind type))
            {
                _ctx.PendingAction = new ActionMessageDto { Type = ActionType.Attack, AttackType = type };
                return true;
            }
            if (key == Constants.CloseMenu)
            {
                _ctx.PendingAction = new ActionMessageDto { Type = ActionType.CloseMenu };
                return true;
            }
            return base.Handle(key);
        }

        public override List<string> GetGuiInstructions()
        {
            if (!_ctx.IsInCombat) return new List<string>();
            return new List<string>
            {
                $" [{FormatKey(Constants.AttackNormal)}] Normal Attack",
                $" [{FormatKey(Constants.AttackStealth)}] Stealth Attack",
                $" [{FormatKey(Constants.AttackMagic)}] Magic Attack"
            };
        }
    }
}
