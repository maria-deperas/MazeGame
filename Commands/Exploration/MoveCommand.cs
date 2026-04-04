using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Exploration
{
    // klasa reprezentująca polecenie ruchu gracza
    public class MoveCommand : ICommand
    {
        private Player _player;
        private Map _map;
        private int _dx;
        private int _dy;

        private MenuManager _menuManager;
        private Action<string> _showMessage;

        public MoveCommand(Player player, Map map, int dx, int dy, MenuManager menuManager, Action<string> showMessage)
        {
            _player = player;
            _map = map;
            _dx = dx;
            _dy = dy;
            _menuManager = menuManager;
            _showMessage = showMessage;
        }
        public string Description => "Move";
        public bool IsVisible => false;

        // Metoda oblicza nową pozycję gracza na podstawie kierunku ruchu i sprawdza, czy jest ona możliwa do wykonania. Jeśli tak, aktualizuje pozycję gracza.
        public void Execute()
        {
            var (nextX, nextY) = _player.GetNextPosition(_dx, _dy);

            // Pobieramy komórkę, na którą gracz próbuje wejść
            Cell targetCell = _map.GetCell(nextX, nextY);

            // Krok 1: Sprawdzamy, czy na polu stoi przeciwnik
            if (targetCell.HasEnemy)
            {
                // Zatrzymujemy ruch i otwieramy Menu Walki!
                _menuManager.ToggleCombatMenu(_player, targetCell.Enemy, targetCell, _showMessage);
            }
            // Krok 2: Jeśli nie ma przeciwnika, standardowo sprawdzamy czy można wejść (czy nie ma ściany)
            else if (_map.IsMovePossible(nextX, nextY))
            {
                _player.Move(nextX, nextY);
            }
        }
    }
}
