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
        public const int BackpackCapacity = 3;
        public const int MaxItemsPerCell = 4;

        // Ustawienia wyświetlania
        public const int StatColumnWidth = 35;       // Szerokość kolumny statystyk
        public const int ActionColumnWidth = 35;     // Szerokość kolumny dostępnych akcji
        public const int EmptySidebarWidth = 75;     // Szerokość pustego pola zamazującego "duchy"
        public const int MenuSeparatorLength = 25;   // Długość kresek oddzielających w menu
        public const int BottomClearLines = 3;       // Ile pustych linii rysować pod mapą
        public const int BottomClearWidth = 80;      // Szerokość linii zamazującej komunikaty
    }
}
