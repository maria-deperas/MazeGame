using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;

namespace MazeGame
{
    public class Cell
    {
        public bool IsWall { get; set; }
        public List<Item> Items { get; } = new List<Item>(); // lista przedmiotów w danej komórce
        public bool HasItem => Items.Count > 0;

        // zwraca odpowiedni symbol do wyświetlenia
        public string GetSymbol()
        {
            if (IsWall)
            {
                return Constants.WallSymbol.ToString();
            }
            if (HasItem)
            {
                return Items.Last().Symbol.ToString().Pastel(Items.Last().ItemColor);
            }

            return Constants.EmptySymbol.ToString();
        }

    }
}
