using MazeGame.Characters.Combat;
using MazeGame.Core;

namespace MazeGame.Commands.Menu
{
    public abstract class BaseCombatCommand : IMenuCommand
    {
        public abstract string Description { get; }
        protected abstract ICombatVisitor GetVisitor();

        public void Execute(GameModel model)
        {
            model.ExecuteAttack(GetVisitor());
        }
    }
}