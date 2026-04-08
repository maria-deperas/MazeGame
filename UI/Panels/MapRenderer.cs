using System.Collections.Generic;
using System.Drawing;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;
using Pastel;

namespace MazeGame.UI.Panels
{
    public class MapRenderer : IRenderable
    {
        private readonly Map _map;
        private readonly Player _player;

        public MapRenderer(Map map, Player player)
        {
            _map = map;
            _player = player;
        }

        public List<string> Render()
        {
            List<string> lines = new List<string>();

            lines.Add("┌" + new string('─', Constants.MazeWidth) + "┐");

            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                string row = "│";
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    if (x == _player.X && y == _player.Y)
                    {
                        row += Constants.PlayerSymbol.ToString().Pastel(Color.Red);
                    }
                    else
                    {
                        row += _map.GetCell(x, y).GetSymbol();
                    }
                }
                row += "│";
                lines.Add(row);
            }

            lines.Add("└" + new string('─', Constants.MazeWidth) + "┘");

            return lines;
        }
    }
}