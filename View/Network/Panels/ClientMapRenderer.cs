using System.Drawing;
using MazeGame.Core;
using MazeGame.Network.Dto;
using Pastel;

namespace MazeGame.UI.Panels
{
    public class ClientMapRenderer : IRenderable
    {
        private readonly Func<GameStateDto?> _getState;
        private readonly Func<int> _getLocalId;

        public ClientMapRenderer(Func<GameStateDto?> getState, Func<int> getLocalId)
        {
            _getState = getState;
            _getLocalId = getLocalId;
        }

        public List<string> Render()
        {
            var lines = new List<string>();
            var state = _getState();

            lines.Add("┌" + new string('─', Constants.MazeWidth) + "┐");

            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                string row = "│";
                for (int x = 0; x < Constants.MazeWidth; x++)
                    row += state != null ? GetCellDisplay(state, x, y) : Constants.EmptySymbol.ToString();
                row += "│";
                lines.Add(row);
            }

            lines.Add("└" + new string('─', Constants.MazeWidth) + "┘");
            return lines;
        }

        private string GetCellDisplay(GameStateDto state, int x, int y)
        {
            int localId = _getLocalId();

            // Lokalny gracz — czerwony
            var me = state.Players.FirstOrDefault(p => p.PlayerId == localId);
            if (me != null && me.X == x && me.Y == y)
                return Constants.PlayerSymbol.ToString().Pastel(Color.Red);

            // Inni gracze — żółte cyfry
            var other = state.Players.FirstOrDefault(p => p.PlayerId != localId && p.X == x && p.Y == y);
            if (other != null)
                return (other.PlayerId % 10).ToString().Pastel(Color.Yellow);

            if (y >= state.Grid.Count || x >= state.Grid[y].Count)
                return Constants.EmptySymbol.ToString();

            var cell = state.Grid[y][x];
            if (cell.IsWall) return Constants.WallSymbol.ToString();
            if (cell.Enemy != null)
                return cell.Enemy.Symbol.ToString().Pastel(HexToColor(cell.Enemy.ColorHex));
            if (cell.Items.Count > 0)
                return cell.Items[^1].Symbol.ToString();

            return Constants.EmptySymbol.ToString();
        }

        private static Color HexToColor(string? hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length < 7) return Color.White;
            try
            {
                int r = Convert.ToInt32(hex.Substring(1, 2), 16);
                int g = Convert.ToInt32(hex.Substring(3, 2), 16);
                int b = Convert.ToInt32(hex.Substring(5, 2), 16);
                return Color.FromArgb(r, g, b);
            }
            catch { return Color.White; }
        }
    }
}
