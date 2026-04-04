using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.World;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class GroundItemsPanel : FramedPanel
    {
        private readonly Map _map;
        private readonly Player _player;
        private readonly MenuManager _menu;

        public GroundItemsPanel(Map map, Player player, MenuManager menu)
            : base(Constants.RightPanelWidth, Constants.MaxItemsPerCell, "ON GROUND")
        { _map = map; _player = player; _menu = menu; }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            var items = _map.GetCell(_player.X, _player.Y).Items;

            for (int i = 0; i < Constants.MaxItemsPerCell; i++)
            {
                if (i < items.Count)
                {
                    string pfx = (_menu.CurrentMenu == MenuMode.Ground && i == _menu.SelectionIndex) ? "> " : "  ";
                    lines.Add($"{pfx}{items[i].GetInfo()}");
                }
                else if (i == 0) lines.Add("  No items here.");
                else lines.Add("");
            }
            return lines;
        }
    }
}