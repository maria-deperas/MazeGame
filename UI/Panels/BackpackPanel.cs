using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class BackpackPanel : FramedPanel
    {
        private readonly Player _player;
        private readonly MenuManager _menu;

        public BackpackPanel(Player player, MenuManager menu)
            : base(Constants.RightPanelWidth, Constants.BackpackCapacity, "BACKPACK")
        { _player = player; _menu = menu; }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < Constants.BackpackCapacity; i++)
            {
                if (i < _player.Backpack.Count)
                {
                    string pfx = (_menu.CurrentMenu == MenuMode.Backpack && i == _menu.SelectionIndex) ? "> " : "  ";
                    lines.Add($"{pfx}{i + 1}. {_player.Backpack[i].GetInfo()}");
                }
                else lines.Add($"  {i + 1}. [ Empty ]");
            }
            return lines;
        }
    }
}