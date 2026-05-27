namespace MazeGame.Events;

public class SpeciesPublisher
{
    public string Name { get; }
    private HashSet<ISpeciesSubscriber> _subscribers = new HashSet<ISpeciesSubscriber>();

    public SpeciesPublisher(string name)
    {
        Name = name;
    }

    public void Subscribe(ISpeciesSubscriber subscriber) => _subscribers.Add(subscriber);
    public void Unsubscribe(ISpeciesSubscriber subscriber) => _subscribers.Remove(subscriber);

    public void NotifySubscribers()
    {
        foreach (var sub in new List<ISpeciesSubscriber>(_subscribers))
        {
            sub.UpdateOnDeath();
        }
    }
}