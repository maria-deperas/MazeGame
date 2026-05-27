using MazeGame.Characters.Combat;

namespace MazeGame.Commands.Menu
{
    public class NormalAttackCommand : BaseCombatCommand
    {
        public override string Description => "Normal Attack";
        protected override ICombatVisitor GetVisitor() => new NormalAttack();
    }
}