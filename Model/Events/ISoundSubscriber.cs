namespace MazeGame.Events;

public interface ISoundSubscriber
{
    int X { get; }
    int Y { get; }
    void UpdateOnSound(int sourceX, int sourceY, int distance);
}