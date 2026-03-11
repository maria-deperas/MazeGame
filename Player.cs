using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Player
    {
        public int x { get; set; }
        public int y { get; set; }
        public Attributes Stats { get; set; }
        public Item LeftHand { get; set; }
        public Item RightHand { get; set; }
        public List<Item> Backpack { get; } = new List<Item>();
        public int Coins { get; set; } = 0;
        public int Gold { get; set; } = 0;

        public Player(int startx, int starty)
        {
            x = startx;
            y = starty;
            Stats = new Attributes(10, 10, 10, 10, 10, 10);
        }

        // metoda sprawdzająca pozycję gracza po wciśnięciu klawisza
        public (int newx, int newy) NextPosition(char key)
        {
            int newx = x;
            int newy = y;

            switch (char.ToLower(key))
            {
                case 'w': 
                    newy--; break;
                case 's': 
                    newy++; break;
                case 'a': 
                    newx--; break;
                case 'd': 
                    newx++; break;
            }
            return (newx, newy);
        }

        // zmienia pozycję gracza
        public void Move(int newx, int newy)
        {
            x = newx;
            y = newy;
        }

        // sprawdza, czy w plecaku gracza jest jeszcze miejsce i wkłada przedmiot do plecaka
        public bool AddToBackpack(Item item)
        {
            // potrzebne gdy zamieniamy null z ręki z przedmiotem w plecaku
            if (item == null) return true;
            if (Backpack.Count < Constants.BackpackCapacity)
            {
                Backpack.Add(item);
                return true;
            }
            return false;
        }

        // usuwanie z plecaka danego przedmiotu
        public bool RemoveFromBackpack(Item item)
        {
            return Backpack.Remove(item);
        }

        // sprawdza, czy w plecaku jest jeszcze miejsce
        public bool CanFitInBackpack(Item item)
        {
            if (item == null) return true;
            return Backpack.Count < Constants.BackpackCapacity;
        }
    }
}
