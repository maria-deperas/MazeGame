using System;

namespace MazeGame.UI
{
    public static class GameOverScreen
    {
        public static void Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
           ____    _    __  __ _____    _____     _______ ____  
          / ___|  / \  |  \/  | ____|  / _ \ \   / / ____|  _ \ 
         | |  _  / _ \ | |\/| |  _|   | | | \ \ / /|  _| | |_) |
         | |_| |/ ___ \| |  | | |___  | |_| |\ V / | |___|  _ < 
          \____/_/   \_\_|  |_|_____|  \___/  \_/  |_____|_| \_\
            ");

            Console.ResetColor();
            Console.WriteLine("\n  Press any key to end the game...");

            Console.ReadKey(true);
        }
    }
}