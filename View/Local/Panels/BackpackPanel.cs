using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class BackpackPanel : FramedPanel
    {
        private readonly Player _player;
        private readonly GameModel _model;

        public BackpackPanel(Player player, GameModel model)
            : base(Constants.RightPanelWidth, Constants.BackpackCapacity, "BACKPACK")
        { _player = player; _model = model; }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < Constants.BackpackCapacity; i++)
            {
                if (i < _player.Backpack.Count)
                {
                    string pfx = (_model.CurrentMenu == MenuMode.Backpack && i == _model.SelectionIndex) ? "> " : "  ";
                    lines.Add($"{pfx}{i + 1}. {_player.Backpack[i].GetInfo()}");
                }
                else lines.Add($"  {i + 1}. [ Empty ]");
            }
            return lines;
        }
    }
}