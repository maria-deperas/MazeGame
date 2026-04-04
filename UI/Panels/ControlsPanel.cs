using System.Collections.Generic;
using MazeGame.UI;
using MazeGame.Core;
using MazeGame.Commands.Exploration;

namespace MazeGame.UI.Panels
{
    public class ControlsPanel : FramedPanel
    {
        private readonly MenuManager _menu;
        private readonly IInputHandler _inputChain;

        // UWAGA: Konstruktor przyjmuje teraz łańcuch IInputHandler!
        public ControlsPanel(MenuManager menu, IInputHandler inputChain)
            : base(Constants.LeftPanelWidth, 10, "CONTROLS")
        {
            _menu = menu;
            _inputChain = inputChain;
        }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();

            if (_menu.CurrentMenu == MenuMode.None)
            {
                // 1. TRYB GRY: Pobieranie z Chain of Responsibility
                var current = _inputChain;
                while (current != null)
                {
                    lines.AddRange(current.GetGuiInstructions());
                    current = current.GetNext();
                }
            }
            else
            {
                // 2. TRYB MENU (Ground / Backpack): Pobieranie prosto ze słowników MenuManagera!
                var activeActions = _menu.GetActiveMenuInfo();
                foreach (var action in activeActions)
                {
                    // Możemy ładnie sformatować strzałki, żeby nie wyświetlało się "UpArrow"
                    string keyName = action.Key;
                    if (keyName == "UpArrow") keyName = "↑";
                    if (keyName == "DownArrow") keyName = "↓";

                    lines.Add($" [{keyName}] {action.Description}");
                }
            }

            return lines;
        }
    }
}