using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Map
    {
        private Cell[,] _cells;

        // rozmieszczenie ścian
        private string[] _layout = new string[]
         {
            //0         10        20        30       39
            "  11111111111111111111111111111111111111", // 0
            "          11              11          11", // 1
            "          11    111111    11          11", // 2
            "          11    11        11          11", // 3
            "111111    11    11        11    11    11", // 4
            "11        11    11     11111    11    11", // 5
            "11              11              11      ", // 6
            "11              11              11      ", // 7
            "11              11    111111    11      ", // 8
            "11    111111111111    11        111111  ", // 9
            "11    11              11                ", // 10
            "11    11              11                ", // 11
            "11    11              11                ", // 12
            "11    11    111111111111111111   11111  ", // 13
            "11          11          11          11  ", // 14
            "11          11          11          11  ", // 15
            "11111111111111    11    11    1111  11  ", // 16
            "                  11    11    11    11  ", // 17
            "                  11          11        ", // 18
            "          1111111111          11        "  // 19
         };
        public Map()
        {
            _cells = new Cell[Constants.MazeHeight, Constants.MazeWidth];
            InitializeMap();
            PlaceItems();
        }

        public Cell GetCell(int x, int y)
        {
            return _cells[y, x];
        }

        // tworzenie ścian
        private void InitializeMap()
        {
            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    _cells[y, x] = new Cell();
                    if (_layout[y][x] == '1') _cells[y, x].IsWall = true;
                }
            }
        }

        // rozmieszczanie przedmiotów
        private void PlaceItems()
        {
            _cells[1, 1].Items.Add(new Rope());
            _cells[1, 1].Items.Add(new OldMap());
            _cells[1, 1].Items.Add(new Rope());

            _cells[2, 5].Items.Add(new Dagger());
            _cells[2, 5].Items.Add(new OldMap());

            _cells[3, 3].Items.Add(new Gold(10));
            _cells[3, 3].Items.Add(new Key());
            _cells[3, 3].Items.Add(new Dagger());

            _cells[10, 5].Items.Add(new Sword());

            _cells[17, 2].Items.Add(new Dagger());
            _cells[17, 2].Items.Add(new Rope());
            _cells[17, 2].Items.Add(new Coin(8));

            _cells[5, 15].Items.Add(new Saber());

            _cells[8, 25].Items.Add(new Saber());
            _cells[8, 25].Items.Add(new Gold(25));

            _cells[12, 12].Items.Add(new Gold(100));
            _cells[12, 12].Items.Add(new Key());

            _cells[1, 35].Items.Add(new Coin(15));
            _cells[1, 35].Items.Add(new OldMap());

            _cells[7, 30].Items.Add(new Sword());
            _cells[7, 30].Items.Add(new Rope());

            _cells[15, 20].Items.Add(new Saber());
            _cells[15, 20].Items.Add(new Coin(5));

            _cells[18, 38].Items.Add(new Gold(50));
            _cells[18, 38].Items.Add(new Coin(20));
        }

        // sprawdza, czy pole nie jest ścianą i czy nie jest poza granicami mapy
        public bool IsMovePossible(int x, int y)
        {
            if (x < 0 || x >= Constants.MazeWidth || y < 0 || y >= Constants.MazeHeight) 
                return false;
            if (_cells[y, x].IsWall)
                return false;
            return true;
        }
    }
}

