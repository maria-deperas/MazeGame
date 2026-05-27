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

        // Klawisze
        public const ConsoleKey MoveUp = ConsoleKey.W;
        public const ConsoleKey MoveDown = ConsoleKey.S;
        public const ConsoleKey MoveLeft = ConsoleKey.A;
        public const ConsoleKey MoveRight = ConsoleKey.D;
        public const ConsoleKey OpenGround = ConsoleKey.E;
        public const ConsoleKey OpenBackpack = ConsoleKey.B;
        public const ConsoleKey OpenLog = ConsoleKey.J;
        public const ConsoleKey PickUp = ConsoleKey.Enter;
        public const ConsoleKey SelectUp = ConsoleKey.UpArrow;
        public const ConsoleKey SelectDown = ConsoleKey.DownArrow;
        public const ConsoleKey EquipLeft = ConsoleKey.L;
        public const ConsoleKey EquipRight = ConsoleKey.R;
        public const ConsoleKey UnequipLeft = ConsoleKey.D1;
        public const ConsoleKey UnequipRight = ConsoleKey.D2;
        public const ConsoleKey DropItem = ConsoleKey.D;
        public const ConsoleKey CloseMenu = ConsoleKey.Escape;
        public const ConsoleKey AttackNormal = ConsoleKey.D1;
        public const ConsoleKey AttackStealth = ConsoleKey.D2;
        public const ConsoleKey AttackMagic = ConsoleKey.D3;
    }
}
