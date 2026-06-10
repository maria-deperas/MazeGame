using MazeGame.World;

namespace MazeGame.Characters.Enemies.Behaviors
{
    // interfejs strategii
    public interface IEnemyBehavior
    {
        // x, y - współrzędne celu ruchu
        (int x, int y)? GetMove(Enemy enemy, List<(int x, int y)> validMoves,
            List<(int x, int y)> playerPositions, Map map);
    }
}