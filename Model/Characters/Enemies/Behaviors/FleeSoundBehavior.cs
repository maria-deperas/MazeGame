using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    public class FleeSoundBehavior : IEnemyBehavior
    {
        public (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map)
        {
            if (enemy.LastHeardSound == null) return null;

            var source = enemy.LastHeardSound.Value;

            var safeMoves = validMoves.Where(move =>
                Distance(move.x, move.y, source.x, source.y) >
                Distance(enemy.X, enemy.Y, source.x, source.y)
            ).ToList();

            if (safeMoves.Count == 0) return null; // powrót do losowego poruszania się

            return safeMoves[new Random().Next(safeMoves.Count)];
        }

        private int Distance(int x1, int y1, int x2, int y2) =>
            Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }
}