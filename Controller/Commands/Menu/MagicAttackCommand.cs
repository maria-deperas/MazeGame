using MazeGame.Characters.Combat;

namespace MazeGame.Commands.Menu
{
    public class MagicAttackCommand : BaseCombatCommand
    {
        public override string Description => "Magic Attack";
        protected override ICombatVisitor GetVisitor() => new MagicAttack();
    }
}