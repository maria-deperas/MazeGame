using System.Drawing;
using Pastel;

namespace MazeGame.Characters
{
    public class Enemy
    {
        public string Name { get; set; }
        public EnemyStats Stats { get; set; }

        public char Symbol { get; set; } = 'E';
        public Color Color { get; set; } = Color.Red;

        public Enemy(string name, EnemyStats stats)
        {
            Name = name;
            Stats = stats;
        }

        public string GetColoredSymbol()
        {
            return Symbol.ToString().Pastel(Color);
        }
    }
}