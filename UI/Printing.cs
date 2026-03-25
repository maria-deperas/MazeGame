using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;
using Pastel;

namespace MazeGame.UI
{
    public class Printing
    {
        private Statistics statsDisplay = new Statistics();
        private ActionPanel actionDisplay = new ActionPanel();

        // główna funkcja rysująca cały interfejs - mapę, statystyki i dostępne akcje
        public void Draw(Map map, Player player, MenuManager menu)
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder();

            Cell currentCell = map.GetCell(player.X, player.Y);

            // Pobieramy linie do wyświetlenia dla statystyk i akcji
            List<string> state = statsDisplay.GetStatLines(player, currentCell, menu.CurrentMenu, menu.SelectionIndex);
            List<string> actions = actionDisplay.GetActionLines(map, player, menu);

            sb.Append("┌" + new string('─', Constants.MazeWidth) + "┐\n");

            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                // Rysowanie mapy
                sb.Append("│");
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    if (x == player.X && y == player.Y)
                        sb.Append(Constants.PlayerSymbol.ToString().Pastel(Color.Red));
                    else
                        sb.Append(map.GetCell(x, y).GetSymbol());
                }
                sb.Append("│");

                string statLine = y < state.Count ? state[y] : "";
                string actionLine = y < actions.Count ? actions[y] : "";

                // Rysowanie statystyk i akcji
                if (!string.IsNullOrEmpty(statLine) || !string.IsNullOrEmpty(actionLine))
                {
                    sb.Append("  ");

                    int statPadding = Math.Max(0, Constants.StatColumnWidth - statLine.Length);
                    sb.Append(statLine);
                    sb.Append(new string(' ', statPadding));

                    sb.Append("  │  ");
                    sb.Append(actionLine);

                    int actionPadding = Math.Max(0, Constants.ActionColumnWidth - actionLine.Length);
                    sb.Append(new string(' ', actionPadding));
                }
                else
                {
                    sb.Append(new string(' ', Constants.EmptySidebarWidth));
                }

                sb.AppendLine();
            }

            sb.Append("└" + new string('─', Constants.MazeWidth) + "┘\n");

            for (int i = 0; i < Constants.BottomClearLines; i++)
            {
                sb.AppendLine(new string(' ', Constants.BottomClearWidth));
            }

            Console.Write(sb.ToString());
        }

        // funkcja do wyświetlania komunikatów
        public void ShowMessage(string msg)
        {
            Console.SetCursorPosition(0, Constants.MazeHeight + 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg.PadRight(Console.WindowWidth));
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}