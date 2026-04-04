using System;

namespace MazeGame.Characters
{
    public class EnemyStats
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }

        public EnemyStats(int maxHealth, int attack, int armor)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Attack = attack;
            Armor = armor;
        }
    }
}