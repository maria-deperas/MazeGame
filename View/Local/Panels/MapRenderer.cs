using System.Drawing;
using MazeGame.Core;
using Pastel;

namespace MazeGame.UI.Panels
{
    public class MapRenderer : IRenderable
    {
        private readonly GameModel _model;
        public MapRenderer(GameModel model)
        {
            _model = model;
        }

        public List<string> Render()
        {
            var lines = new List<string>();

            lines.Add("┌" + new string('─', Constants.MazeWidth) + "┐");

            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                string row = "│";
                for (int x = 0; x < Constants.MazeWidth; x++)
                    row += GetSymbolAt(x, y);
                row += "│";
                lines.Add(row);
            }

            lines.Add("└" + new string('─', Constants.MazeWidth) + "┘");

            return lines;
        }

        private string GetSymbolAt(int x, int y)
        {
            if (_model.Player != null && x == _model.Player.X && y == _model.Player.Y)
                return Constants.PlayerSymbol.ToString().Pastel(Color.Red);

            foreach (var (id, remote) in _model.RemotePlayers)
            {
                if (remote.X == x && remote.Y == y)
                    return (id % 10).ToString().Pastel(Color.Yellow);
            }

            return _model.Map.GetCell(x, y).GetSymbol();
        }
    }
}