using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class CombatPanel : FramedPanel
    {
        private readonly GameModel _model;

        public CombatPanel(GameModel model) : base(Constants.RightPanelWidth, 4, "COMBAT")
        {
            _model = model;
        }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            Enemy? enemy = _model.ActiveEnemy;

            if (_model.CurrentMenu == MenuMode.Combat && enemy != null)
            {
                lines.Add($"Enemy: {enemy.Name}");
                lines.Add($"HP:         {enemy.Stats.CurrentHealth} / {enemy.Stats.MaxHealth}");
                lines.Add($"Attack:       {enemy.Stats.Attack}");
                lines.Add($"Armor:    {enemy.Stats.Armor}");
            }
            else
            {
                lines.Add("No enemy.");
            }

            return lines;
        }
    }
}