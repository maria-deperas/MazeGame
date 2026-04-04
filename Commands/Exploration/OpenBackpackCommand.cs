using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Exploration
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
        public string Description => "Open backpack";
        public bool IsVisible => _player.Backpack.Count > 0 || _player.LeftHand != null || _player.RightHand != null;

        // metoda wywoływana, gdy użytkownik wciśnie 'B' - otwieranie menu plecaka
        public void Execute()
        {
            _menuManager.ToggleBackpackMenu(_player);
        }
    }
}
