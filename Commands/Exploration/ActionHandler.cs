using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Exploration
{
    // klasa odpowiedzialna za obsługę akcji związanych z interakcją z otoczeniem - otwieranie menu pola i plecaka
    public class ActionHandler : BaseInputHandler
    {
        private Player _player;
        private Map _map;

        private Dictionary<ConsoleKey, ICommand> _keyBindings; // słownik, który mapuje klawisze do konkretnych poleceń

        public ActionHandler(Player player, Map map, MenuManager menuManager, Action<string> showMessage)
        {
            _keyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { ConsoleKey.E, new OpenGroundMenuCommand(menuManager, player, map, showMessage) },
                { ConsoleKey.B, new OpenBackpackCommand(menuManager, player) },
                { ConsoleKey.J, new OpenLogCommand() }
            };
        }

        // Metoda Handle sprawdza, czy wciśnięty klawisz jest jednym z tych, które obsługujemy
        public override bool Handle(ConsoleKey key)
        {
            if (_keyBindings.TryGetValue(key, out ICommand command))
            {
                command.Execute();
                return true;
            }

            return base.Handle(key);
        }

        // Nadpisujemy metodę z BaseInputHandler
        public override List<string> GetGuiInstructions()
        {
            var instructions = new List<string>();

            foreach (var binding in _keyBindings)
            {
                if (binding.Value.IsVisible)
                {
                    instructions.Add($" [{binding.Key}] {binding.Value.Description}");
                }
            }

            return instructions;
        }
    }
}
