using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Logging;

namespace MazeGame.UI
{
    public static class LogScreen
    {
        public static void Show()
        {
            Console.Clear();
            Console.WriteLine(" EVENT LOG\n");

            var history = GameLog.GetInstance().Memory?.GetFullHistory() ?? new List<string>();
            var displayLogs = history.Skip(Math.Max(0, history.Count - 25)).ToList();

            if (history.Count == 0)
            {
                Console.WriteLine(" Log is empty.");
            }
            else
            {
                foreach (var log in displayLogs)
                {
                    Console.WriteLine($" {log}");
                }
            }

            Console.WriteLine("\n Press any key to return to the game...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
