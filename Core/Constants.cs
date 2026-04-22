using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Core
{
    public static class Constants
    {
        // Wymiary labiryntu
        public const int MazeWidth = 40;
        public const int MazeHeight = 20;

        // Symbole
        public const char PlayerSymbol = '¶';
        public const char WallSymbol = '█';
        public const char EmptySymbol = ' ';

        // Pojemność plecaka i ilość przedmiotów na ziemi
        public const int BackpackCapacity = 4;
        public const int MaxItemsPerCell = 4;

        public const int LeftPanelWidth = 20;
        public const int RightPanelWidth = 50; 
        public const int PanelHeight = MazeHeight + 4;
    }
}
