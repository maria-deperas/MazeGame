using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Core;
using MazeGame.Events;
using MazeGame.Characters;

namespace MazeGame.World
{
    public class Map
    {
        private Cell[,] _cells;

        // Lista przechowująca instrukcje dla tej konkretnej mapy
        public List<string> Instructions { get; private set; }
        
        public SoundPublisher? SoundManager { get; private set; }
        public Map(Cell[,] cells, List<string> instructions, SoundPublisher? soundPublisher = null)
        {
            _cells = cells;
            Instructions = instructions;
            SoundManager = soundPublisher;
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

        public void MoveEnemies(IEnumerable<(int x, int y)>? playerPositions, IEnumerable<Enemy>? lockedEnemies = null)
        {
            Random rnd = new Random();
            var locked  = lockedEnemies  != null ? new HashSet<Enemy>(lockedEnemies)         : null;
            var blocked = playerPositions != null ? new HashSet<(int, int)>(playerPositions) : null;
            var playerList = playerPositions?.ToList() ?? new List<(int x, int y)>();

            List<Enemy> enemiesToMove = new List<Enemy>();
            for (int y = 0; y < Constants.MazeHeight; y++)
            for (int x = 0; x < Constants.MazeWidth; x++)
                if (_cells[y, x].HasEnemy)
                    enemiesToMove.Add(_cells[y, x].Enemy!);

            (int dx, int dy)[] directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };

            foreach (var enemy in enemiesToMove)
            {
                if (locked != null && locked.Contains(enemy)) continue;

                List<(int x, int y)> validMoves = new List<(int, int)>();
                foreach (var dir in directions)
                {
                    int nx = enemy.X + dir.dx;
                    int ny = enemy.Y + dir.dy;
                    if (IsMovePossible(nx, ny) && !_cells[ny, nx].HasEnemy
                                               && !(blocked != null && blocked.Contains((nx, ny))))
                        validMoves.Add((nx, ny));
                }

                if (validMoves.Count == 0) continue;

                // gracz > dźwięk > losowy
                (int x, int y)? move = null;

                // gracz
                move = enemy.PlayerBehavior.GetMove(enemy, validMoves, playerList, this);

                // dźwięk
                if (move == null)
                    move = enemy.SoundBehavior.GetMove(enemy, validMoves, playerList, this);

                // losowy ruch
                if (move == null && rnd.Next(100) < 75)
                    move = validMoves[rnd.Next(validMoves.Count)];

                if (move != null)
                {
                    _cells[enemy.Y, enemy.X].Enemy = null;
                    enemy.X = move.Value.x;
                    enemy.Y = move.Value.y;
                    _cells[enemy.Y, enemy.X].Enemy = enemy;
                }
            }
        }
    }
}

