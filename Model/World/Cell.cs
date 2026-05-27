using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.Items;
using Pastel;

namespace MazeGame.World
{
    public class Cell
    {
        public bool IsWall { get; set; }
        public List<Item> Items { get; } = new List<Item>(); // lista przedmiotów w danej komórce
        public bool HasItem => Items.Count > 0;
        public Enemy? Enemy { get; set; }
        public bool HasEnemy => Enemy != null;

        // zwraca odpowiedni symbol do wyświetlenia
        public string GetSymbol()
        {
            if (IsWall)
            {
                return Constants.WallSymbol.ToString();
            }
            if (HasEnemy)
            {
                return Enemy!.GetColoredSymbol();
            }
            if (HasItem)
            {
                return Items.Last().Symbol.ToString().Pastel(Items.Last().ItemColor);
            }

            return Constants.EmptySymbol.ToString();
        }

    }
}
