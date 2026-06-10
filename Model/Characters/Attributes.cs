using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Characters
{
    public class Attributes
    {
        public int Strength { get; set; } // siła
        public int Agility { get; set; } // zręczność
        public int Health { get; set; } // zdrowie
        public int Luck { get; set; } // szczęście
        public int Aggression { get; set; } // agresja
        public int Wisdom { get; set; } // mądrość

        public Attributes(int str, int agil, int hl, int lu, int agr, int wis)
        {
            Strength = str;
            Agility = agil;
            Health = hl;
            Luck = lu;
            Aggression = agr;
            Wisdom = wis;
        }
        
        // metody do dodawania/odejmowania bonusów
        public void Add(Attributes other)
        {
            Strength += other.Strength;
            Agility += other.Agility;
            Health += other.Health;
            Luck += other.Luck;
            Aggression += other.Aggression;
            Wisdom += other.Wisdom;
        }

        public void Subtract(Attributes other)
        {
            Strength -= other.Strength;
            Agility -= other.Agility;
            Health -= other.Health;
            Luck -= other.Luck;
            Aggression -= other.Aggression;
            Wisdom -= other.Wisdom;
        }
    }
}
