using System;
using System.Collections.Generic;
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

            // Zakładamy, że wszystkie panele mają tę samą liczbę linii (dzięki Constants.PanelHeight)
            int totalLines = leftLines.Count;

            for (int i = 0; i < totalLines; i++)
            {
                // Rysujemy lewą ramkę, odstęp, mapę, odstęp, prawą ramkę
                Console.WriteLine($"{leftLines[i]}  {middleLines[i]}  {rightLines[i]}");
            }
        }

        public void ShowMessage(string msg)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;

            // 1. Dzielimy wiadomość na poszczególne linie, używając znaku nowej linii (\n)
            string[] lines = msg.Split('\n');

            // 2. Każdą linię wypisujemy osobno i wypełniamy spacjami do końca ekranu
            foreach (string line in lines)
            {
                // Używamy Console.WindowWidth - 1, by uniknąć dziwnego przeskakiwania kursora na niektórych konsolach
                Console.WriteLine(line.TrimEnd().PadRight(Console.WindowWidth - 1));
            }

            // 3. "Gumka myszka" - drukujemy 4 puste linie, żeby zmazać resztki po starych komunikatach
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
            }

            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}