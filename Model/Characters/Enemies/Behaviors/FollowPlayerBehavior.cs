using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    public class FollowPlayerBehavior : IEnemyBehavior
    {
        public (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map)
        {
            var (dirX, dirY) = FindPlayerDirection(enemy, playerPositions, map);
            if (dirX == 0 && dirY == 0) return null;

            int targetX = enemy.X + dirX;
            int targetY = enemy.Y + dirY;

            var move = validMoves.FirstOrDefault(m => m.x == targetX && m.y == targetY);
            if (move != default) return move;

            return null;
        }

        // skanuje 4 kierunki i zwraca, w którym kierunku jest gracz
        private (int dx, int dy) FindPlayerDirection(Enemy enemy,
            List<(int x, int y)> playerPositions, Map map)
        {
            int[] dxs = { 0, 0, -1, 1 };
            int[] dys = { -1, 1, 0, 0 };

            for (int i = 0; i < 4; i++)
            for (int step = 1; step <= enemy.VisionRange; step++)
            {
                int cx = enemy.X + dxs[i] * step;
                int cy = enemy.Y + dys[i] * step;
                if (!map.IsMovePossible(cx, cy)) break;
                if (playerPositions.Contains((cx, cy)))
                    return (dxs[i], dys[i]);
            }
            return (0, 0);
        }
    }
}