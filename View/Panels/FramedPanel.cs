using System;
using System.Collections.Generic;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public abstract class FramedPanel : IRenderable
    {
        protected int Width { get; private set; }
        protected int Height { get; private set; }
        protected string Title { get; private set; }

        public FramedPanel(int width, int height, string title)
        {
            Width = width;
            Height = height;
            Title = title;
        }

        protected abstract List<string> GetContentLines();

        public List<string> Render()
        {
            List<string> lines = new List<string>();
            List<string> content = GetContentLines();

            string headerTitle = $" {Title} ";

            int leftPad = (Width - headerTitle.Length) / 2;
            int rightPad = Width - headerTitle.Length - leftPad;

            lines.Add("┌" + new string('─', leftPad - 1) + headerTitle + new string('─', rightPad - 1) + "┐");

            for (int i = 0; i < Height; i++)
            {
                string lineContent = i < content.Count ? content[i] : "";

                if (lineContent.Length > Width - 2)
                    lineContent = lineContent.Substring(0, Width - 2);
                else
                    lineContent = lineContent.PadRight(Width - 2);

                lines.Add("│" + lineContent + "│");
            }

            lines.Add("└" + new string('─', Width - 2) + "┘");

            return lines;
        }
    }
}