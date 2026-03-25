using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.World;

namespace MazeGame.Input
{
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

        // Metoda Handle sprawdza, czy wciśnięty klawisz jest jednym z tych, które obsługujemy
        public override bool Handle(ConsoleKey key)
        {
            if (_keyBindings.TryGetValue(key, out ICommand command))
            {
                // Jeśli tak, po prostu wykonujemy przypisane mu polecenie!
                command.Execute();
                return true;
            }

            return base.Handle(key);
        }
    }
}
