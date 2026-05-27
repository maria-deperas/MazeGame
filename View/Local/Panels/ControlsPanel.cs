using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class ControlsPanel : FramedPanel
    {
        private readonly GameModel _model;
        private readonly Func<List<string>> _getExplorationInstructions;
        private readonly Func<List<(string Key, string Description)>> _getMenuActions;

        public ControlsPanel(
            GameModel model,
            Func<List<string>> getExplorationInstructions,
            Func<List<(string Key, string Description)>> getMenuActions)
            : base(Constants.LeftPanelWidth, 17, "CONTROLS")
        {
            _model = model;
            _getExplorationInstructions = getExplorationInstructions;
            _getMenuActions = getMenuActions;
        }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();

            if (_model.CurrentMenu == MenuMode.None)
            {
                lines.AddRange(_getExplorationInstructions());
            }
            else
            {
                foreach (var action in _getMenuActions())
                {
                    string keyName = action.Key;
                    if (keyName == "UpArrow")   keyName = "↑";
                    if (keyName == "DownArrow") keyName = "↓";
                    lines.Add($" [{keyName}] {action.Description}");
                }
            }

            return lines;
        }
    }
}