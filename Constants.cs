using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public static class Constants
    {
        public const int MazeWidth = 40;
        public const int MazeHeight = 20;

        public const char PlayerSymbol = '¶';
        public const char WallSymbol = '█';
        public const char EmptySymbol = ' ';

        // szerokość wyświetlanych statystyk
        public const int Width = 70;

        public const int BackpackCapacity = 3;
        public const int MaxItemsPerCell = 4;
    }
}
