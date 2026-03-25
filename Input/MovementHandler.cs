using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.World;

namespace MazeGame.Input
{
    // zajmuje się poruszaniem postacią gracza po mapie
    public class MovementHandler : BaseInputHandler
    {
        private Player _player;
        private Map _map;
        private Dictionary<ConsoleKey, ICommand> _keyBindings;

        public MovementHandler(Player player, Map map)
        {
            _keyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { ConsoleKey.W, new MoveCommand(player, map, 0, -1) },
                { ConsoleKey.S, new MoveCommand(player, map, 0, 1) },
                { ConsoleKey.A, new MoveCommand(player, map, -1, 0) },
                { ConsoleKey.D, new MoveCommand(player, map, 1, 0) }
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
    }
}
