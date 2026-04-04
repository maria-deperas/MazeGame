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
    public class OpenGroundMenuCommand : ICommand
    {
        private MenuManager _menuManager;
        private Player _player;
        private Map _map;
        private Action<string> _showMessage;

        public OpenGroundMenuCommand(MenuManager menuManager, Player player, Map map, Action<string> showMessage)
        {
            _menuManager = menuManager;
            _player = player;
            _map = map;
            _showMessage = showMessage;
        }
        public string Description => "Pick up item";
        public bool IsVisible => _map.GetCell(_player.X, _player.Y).HasItem;

        // Metoda Execute wywołuje odpowiednią funkcję w MenuManager, która otwiera menu pola
        public void Execute()
        {
            _menuManager.ToggleGroundMenu(_player, _map, _showMessage);
        }
    }
}
