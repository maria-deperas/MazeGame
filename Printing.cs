using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;

namespace MazeGame
{
    public class Printing
    {
        private Statistics statsDisplay = new Statistics();

        // wyświetlanie w terminalu labiryntu i statystyk
        public void Draw(Map map, Player player, MenuManager menu)
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder();

            Cell currentCell = map.GetCell(player.x, player.y);

            // tworzenie listy wszystkich statystyk - co jest na danym polu, ekwipunek gracza
            List<string> state = statsDisplay.GetStatLines(player, currentCell, menu.CurrentMenu, menu.SelectionIndex);

            sb.Append("┌" + new string('─', Constants.MazeWidth) + "┐\n");

            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                sb.Append("│");
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    if (x == player.x && y == player.y)
                        sb.Append(Constants.PlayerSymbol.ToString().Pastel(Color.Red));
                    else
                        sb.Append(map.GetCell(x, y).GetSymbol());
                }
                sb.Append("│");

                // doklejamy linijkę ze statystykami
                if (y < state.Count) sb.Append("  " + state[y]);
                sb.AppendLine();
            }
            sb.Append("└" + new string('─', Constants.MazeWidth) + "┘\n");

            for (int i = 0; i < 5; i++) sb.AppendLine(new string(' ', Console.WindowWidth));

            Console.Write(sb.ToString());
        }

        // wyświetlanie komunikatów pod labiryntem
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