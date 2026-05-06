using System.Drawing;
using MazeGame.Events;
using Pastel;

namespace MazeGame.Characters
{
    public abstract class Enemy : ISpeciesSubscriber, ISoundSubscriber
    {
        public string Name { get; set; }
        public EnemyStats Stats { get; set; }
        
        public char Symbol { get; set; }
        public Color Color { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        protected SpeciesPublisher _species;
        protected SoundPublisher _sound;

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

        public abstract void UpdateOnSound(int sourceX, int sourceY, int range);
        public abstract void UpdateOnDeath();
    }
}