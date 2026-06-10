using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    // domyślne zachowanie wrogów
    public class IgnoreBehavior : IEnemyBehavior
    {
        public (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map)
        {
            return null;
        }
    }
}