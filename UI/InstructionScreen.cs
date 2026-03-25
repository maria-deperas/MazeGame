using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.UI
{
    // Ekran instrukcji - pokazuje listę instrukcji i czeka, aż użytkownik wciśnie Enter, by zacząć grę
    public class InstructionScreen
    {
        public void ShowAndAwaitStart(List<string> instructions)
        {
            Console.Clear();

            foreach (string line in instructions)
            {
                Console.WriteLine(line);
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            }

            Console.Clear();
        }
    }
}
