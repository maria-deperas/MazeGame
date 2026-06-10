using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    public class FleePlayerBehavior : IEnemyBehavior
    {
        public (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map)
        {
            var playerDirections = FindAllPlayerDirections(enemy, playerPositions, map);
            if (playerDirections.Count == 0) return null;

            // ucieka w przeciwnym kierunku
            var primary = playerDirections[0];
            int fleeX = enemy.X - primary.dx;
            int fleeY = enemy.Y - primary.dy;
            var straightFlee = validMoves.FirstOrDefault(m => m.x == fleeX && m.y == fleeY);
            if (straightFlee != default) return straightFlee;
            
            var safeMoves = validMoves.Where(move =>
                !playerDirections.Any(dir =>
                    move.x == enemy.X + dir.dx && move.y == enemy.Y + dir.dy)
            ).ToList();

            if (safeMoves.Count == 0) return null;
            return safeMoves[new Random().Next(safeMoves.Count)];
        }

        // sprawdza 4 kierunki, zwraca strony, z których znajdują się gracze
        private List<(int dx, int dy)> FindAllPlayerDirections(Enemy enemy,
            List<(int x, int y)> playerPositions, Map map)
        {
            int[] dxs = { 0, 0, -1, 1 };
            int[] dys = { -1, 1, 0, 0 };
            var result = new List<(int dx, int dy)>();

            for (int i = 0; i < 4; i++)
            for (int step = 1; step <= enemy.VisionRange; step++)
            {
                int cx = enemy.X + dxs[i] * step;
                int cy = enemy.Y + dys[i] * step;
                if (!map.IsMovePossible(cx, cy)) break;
                if (playerPositions.Contains((cx, cy)))
                {
                    result.Add((dxs[i], dys[i]));
                    break;
                }
            }
            return result;
        }
    }
}