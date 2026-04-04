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
        public const int BackpackCapacity = 5;
        public const int MaxItemsPerCell = 8;

        // Fragment do dodania/zmiany w Constants.cs
        public const int LeftPanelWidth = 20;  // Dużo miejsca na długie nazwy przedmiotów!
        public const int RightPanelWidth = 50; // Miejsce na statystyki i akcje
        public const int PanelHeight = MazeHeight + 2; // Panele będą tej samej wysokości co mapa
    }
}
