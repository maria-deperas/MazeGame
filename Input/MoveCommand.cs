using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.World;

namespace MazeGame.Input
{
    // klasa reprezentująca polecenie ruchu gracza
    public class MoveCommand : ICommand
    {
        private Player _player;
        private Map _map;
        private int _dx;
        private int _dy;

        public MoveCommand(Player player, Map map, int dx, int dy)
        {
            _player = player;
            _map = map;
            _dx = dx;
            _dy = dy;
        }

        // Metoda oblicza nową pozycję gracza na podstawie kierunku ruchu i sprawdza, czy jest ona możliwa do wykonania. Jeśli tak, aktualizuje pozycję gracza.
        public void Execute()
        {
            var (nextX, nextY) = _player.GetNextPosition(_dx, _dy);
            if (_map.IsMovePossible(nextX, nextY))
            {
                _player.Move(nextX, nextY);
            }
        }
    }
}
