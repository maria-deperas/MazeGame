using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Core;
using MazeGame.Items;

namespace MazeGame.Characters
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Attributes Stats { get; set; }
        public Item LeftHand { get; set; }
        public Item RightHand { get; set; }
        public List<Item> Backpack { get; } = new List<Item>();
        public int Coins { get; set; } = 0;
        public int Gold { get; set; } = 0;

        public Player(int startx, int starty)
        {
            X = startx;
            Y = starty;
            Stats = new Attributes(10, 10, 50, 10, 10, 10);
        }

        // metoda sprawdzająca pozycję gracza po wciśnięciu klawisz
        public (int newX, int newY) GetNextPosition(int dx, int dy)
        {
            return (X + dx, Y + dy);
        }

        // zmienia pozycję gracza
        public void Move(int newx, int newy)
        {
            X = newx;
            Y = newy;
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
