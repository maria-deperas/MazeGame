using System.Collections.Generic;

namespace MazeGame.UI.Panels
{
    public class VerticalStackPanel : IRenderable
    {
        private readonly List<IRenderable> _panels;
        private readonly int _width;
        private readonly int _targetHeight;

        public VerticalStackPanel(int width, int targetHeight, params IRenderable[] panels)
        {
            _width = width;
            _targetHeight = targetHeight;
            _panels = new List<IRenderable>(panels);
        }

        public List<string> Render()
        {
            List<string> lines = new List<string>();

            foreach (var panel in _panels)
            {
                lines.AddRange(panel.Render());
            }

            while (lines.Count < _targetHeight)
            {
                lines.Add(new string(' ', _width));
            }

            return lines;
        }
    }
}