using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    public class FollowSoundBehavior : IEnemyBehavior
    {
        public (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map)
        {
            if (enemy.LastHeardSound == null) return null;

            var target = enemy.LastHeardSound.Value;

            // gdy dotrze do źródła -> resetujemy
            if (enemy.X == target.x && enemy.Y == target.y)
            {
                enemy.LastHeardSound = null;
                return null;
            }

            return validMoves
                .OrderBy(m => Math.Abs(m.x - target.x) + Math.Abs(m.y - target.y))
                .FirstOrDefault();
        }
    }
}