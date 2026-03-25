using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Input
{
    public class OpenBackpackCommand : ICommand
    {
        private MenuManager _menuManager;
        private Player _player;

        public OpenBackpackCommand(MenuManager menuManager, Player player)
        {
            _menuManager = menuManager;
            _player = player;
        }

        // metoda wywoływana, gdy użytkownik wciśnie 'B' - otwieranie menu plecaka
        public void Execute()
        {
            _menuManager.ToggleBackpackMenu(_player);
        }
    }
}
