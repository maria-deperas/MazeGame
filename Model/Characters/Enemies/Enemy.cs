using System.Drawing;
using MazeGame.Events;
using Pastel;
using MazeGame.Characters.Enemies.Behaviors;
using MazeGame.Logging;

namespace MazeGame.Characters
{
    public abstract class Enemy : ISpeciesSubscriber, ISoundSubscriber
    {
        public string Name { get; set; }
        public EnemyStats Stats { get; set; }
        
        public char Symbol { get; set; }
        public Color Color { get; set; }
        
        public int VisionRange { get; protected set; } = 5;
        public (int x, int y)? LastHeardSound { get; set; } // ostatnia usłyszana pozycja dźwięku
        public IEnemyBehavior PlayerBehavior { get; set; } = new IgnoreBehavior(); // reagowanie na gracza
        public IEnemyBehavior SoundBehavior  { get; set; } = new IgnoreBehavior(); // reagowanie na dźwięk

        public int X { get; set; }
        public int Y { get; set; }

        protected SpeciesPublisher? _species;
        protected SoundPublisher? _sound;

        public Enemy(string name, EnemyStats stats, char symbol, Color color)
        {
            Name = name;
            Stats = stats;
            Symbol = symbol;
            Color = color;
        }

        public void InitializeSubscriptions(SpeciesPublisher species, SoundPublisher sound, int startX, int startY)
        {
            _species = species;
            _sound = sound;
            X = startX;
            Y = startY;

            _species?.Subscribe(this);
            _sound?.Subscribe(this);
        }

        public string GetColoredSymbol()
        {
            return Symbol.ToString().Pastel(Color);
        }

        public virtual void Die()
        {
            _species?.NotifySubscribers();
            _species?.Unsubscribe(this);
            _sound?.Unsubscribe(this);
        }

        public virtual void UpdateOnSound(int sourceX, int sourceY, int range)
        {
            LastHeardSound = (sourceX, sourceY);
            GameLog.GetInstance().Log($"{Name} hears something at [{sourceX},{sourceY}].");
        }
        
        public virtual void OnAttacked() { }
        public abstract void UpdateOnDeath();
    }
}