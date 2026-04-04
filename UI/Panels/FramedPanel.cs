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

        // Metoda, którą będą musiały napisać klasy dziedziczące (np. InventoryPanel)
        protected abstract List<string> GetContentLines();

        // Wspólna logika rysowania ramki wokół treści
        public List<string> Render()
        {
            List<string> lines = new List<string>();
            List<string> content = GetContentLines();

            // Górna ramka z wplecionym tytułem
            string headerTitle = $" {Title} ";

            int leftPad = (Width - headerTitle.Length) / 2;
            int rightPad = Width - headerTitle.Length - leftPad;

            lines.Add("┌" + new string('─', leftPad - 1) + headerTitle + new string('─', rightPad - 1) + "┐");

            // Środek ramki
            for (int i = 0; i < Height; i++)
            {
                string lineContent = i < content.Count ? content[i] : "";

                // Obcinamy tekst, jeśli jest za długi, lub dodajemy spacje, jeśli jest za krótki
                if (lineContent.Length > Width - 2)
                    lineContent = lineContent.Substring(0, Width - 2);
                else
                    lineContent = lineContent.PadRight(Width - 2);

                lines.Add("│" + lineContent + "│");
            }

            // Dolna ramka
            lines.Add("└" + new string('─', Width - 2) + "┘");

            return lines;
        }
    }
}