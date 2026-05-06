using MazeGame.World;

namespace MazeGame.Events;

public class SoundPublisher
{
    private HashSet<ISoundSubscriber> _subscribers = new HashSet<ISoundSubscriber>();

    public void Subscribe(ISoundSubscriber subscriber) => _subscribers.Add(subscriber);
    public void Unsubscribe(ISoundSubscriber subscriber) => _subscribers.Remove(subscriber);

    public void NotifySubscribers(int startX, int startY, int maxRange, Map board)
    {
        Queue<(int x, int y, int dist)> queue = new Queue<(int x, int y, int dist)>();
        
        HashSet<(int x, int y)> visited = new HashSet<(int, int)>();

        queue.Enqueue((startX, startY, 0));
        visited.Add((startX, startY));

        (int dx, int dy)[] directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            foreach (var sub in _subscribers)
            {
                if (sub.X == current.x && sub.Y == current.y && current.dist > 0)
                {
                    sub.UpdateOnSound(startX, startY, current.dist);
                }
            }

            if (current.dist >= maxRange) continue;

            foreach (var dir in directions)
            {
                int nx = current.x + dir.dx;
                int ny = current.y + dir.dy;

                if (board.IsMovePossible(nx, ny) && !visited.Contains((nx, ny)))
                {
                    visited.Add((nx, ny));
                    queue.Enqueue((nx, ny, current.dist + 1)); 
                }
            }
        }
    }
}