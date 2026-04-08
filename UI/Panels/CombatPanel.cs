using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class CombatPanel : FramedPanel
    {
        private readonly MenuManager _menu;

        // Przekazujemy szerokość, wysokość i tytuł ramki do klasy bazowej
        public CombatPanel(MenuManager menu) : base(Constants.RightPanelWidth, 4, "COMBAT")
        {
            _menu = menu;
        }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            Enemy enemy = _menu.ActiveEnemy;

            if (_menu.CurrentMenu == MenuMode.Combat && enemy != null)
            {
                lines.Add($"Enemy: {enemy.Name} {enemy.GetColoredSymbol()}");
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