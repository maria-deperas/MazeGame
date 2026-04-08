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
    // zajmuje się poruszaniem postacią gracza po mapie
    public class MovementHandler : BaseInputHandler
    {
        private Player _player;
        private Map _map;
        private Dictionary<ConsoleKey, ICommand> _keyBindings;

        public MovementHandler(Player player, Map map, MenuManager menuManager, Action<string> showMessage)
        {
            _keyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { ConsoleKey.W, new MoveCommand(player, map, 0, -1, menuManager, showMessage) },
                { ConsoleKey.S, new MoveCommand(player, map, 0, 1, menuManager, showMessage) },
                { ConsoleKey.A, new MoveCommand(player, map, -1, 0, menuManager, showMessage) },
                { ConsoleKey.D, new MoveCommand(player, map, 1, 0, menuManager, showMessage) }
            };
        }

        // metoda sprawdza, czy klawisz jest w słowniku i jeśli tak, wykonuje odpowiadające mu polecenie ruchu. Jeśli klawisz nie jest obsługiwany, przekazuje go dalej
        public override bool Handle(ConsoleKey key)
        {
            if (_keyBindings.TryGetValue(key, out ICommand command))
            {
                command.Execute();
                return true;
            }

            return base.Handle(key);
        }
        
        public override List<string> GetGuiInstructions()
        {
            var keys = string.Join("/", _keyBindings.Keys);
            return new List<string> { $" [{keys}] Move" };
        }
    }
}
