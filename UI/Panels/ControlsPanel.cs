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

        public ControlsPanel(MenuManager menu, IInputHandler inputChain)
            : base(Constants.LeftPanelWidth, 8, "CONTROLS")
        {
            _menu = menu;
            _inputChain = inputChain;
        }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();

            if (_menu.CurrentMenu == MenuMode.None)
            {
                var current = _inputChain;
                while (current != null)
                {
                    lines.AddRange(current.GetGuiInstructions());
                    current = current.GetNext();
                }
            }
            else
            {
                var activeActions = _menu.GetActiveMenuInfo();
                foreach (var action in activeActions)
                {
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