using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MazeGame.Core;

namespace MazeGame.World
{
    public class Map
    {
        private Cell[,] _cells;

        // Lista przechowująca instrukcje dla tej konkretnej mapy
        public List<string> Instructions { get; private set; }
        public Map(Cell[,] cells, List<string> instructions)
        {
            _cells = cells;
            Instructions = instructions;
        }

        public Cell GetCell(int x, int y)
        {
            return _cells[y, x];
        }

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

