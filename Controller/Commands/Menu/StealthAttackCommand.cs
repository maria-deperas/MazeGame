using MazeGame.Characters.Combat;

namespace MazeGame.Commands.Menu
{
    public class StealthAttackCommand : BaseCombatCommand
    {
        public override string Description => "Stealth Attack";
        protected override ICombatVisitor GetVisitor() => new StealthAttack();
    }
}