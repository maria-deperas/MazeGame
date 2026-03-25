using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Input
{
    // Handler odpowiedzialny za przekierowywanie klawiszy do MenuManagera, gdy jesteśmy w trybie menu
    public class MenuInputHandler : BaseInputHandler
    {
        private MenuManager _menuManager;
        private Player _player;
        private Map _map;
        private Action<string> _showMessage;
        
        public MenuInputHandler(MenuManager menuManager, Player player, Map map, Action<string> showMessage)
        {
            _menuManager = menuManager;
            _player = player;
            _map = map;
            _showMessage = showMessage;
        }

        public override bool Handle(ConsoleKey key)
        {
            // Jeśli jesteśmy w trybie menu, przekierowujemy klawisz do MenuManagera
            if (_menuManager.IsInMenuMode)
            {
                _menuManager.HandleMenuInput(_player, _map, key, _showMessage);
                return true;
            }

            // Jeśli nie jesteśmy w trybie menu, przekazujemy klawisz dalej w łańcuchu
            return base.Handle(key);
        }
    }
}
