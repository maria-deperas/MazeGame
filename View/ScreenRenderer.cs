using System;
using System.Collections.Generic;
using MazeGame.Core;
using MazeGame.UI.Panels;

namespace MazeGame.UI
{
    public class ScreenRenderer
    {
        public void DrawScreen(IRenderable leftPanel, IRenderable middlePanel, IRenderable rightPanel)
        {
            Console.SetCursorPosition(0, 0);

            List<string> leftLines = leftPanel.Render();
            List<string> middleLines = middlePanel.Render();
            List<string> rightLines = rightPanel.Render();

            int totalLines = Math.Max(leftLines.Count, Math.Max(middleLines.Count, rightLines.Count));

            for (int i = 0; i < totalLines; i++)
            {
                string l = i < leftLines.Count ? leftLines[i] : new string(' ', Constants.LeftPanelWidth);
                string m = i < middleLines.Count ? middleLines[i] : new string(' ', Constants.MazeWidth + 2);
                string r = i < rightLines.Count ? rightLines[i] : "";

                Console.WriteLine($"{l}  {m}  {r}");
            }
        }

        public void ShowMessage(string msg)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;

            string[] lines = msg.Split('\n');

            foreach (string line in lines)
            {
                Console.WriteLine(line.TrimEnd().PadRight(Console.WindowWidth - 1));
            }

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
            }

            Console.ResetColor();
        }
    }
}