using System;
using System.Text;
using MazeGame;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        Map map = new Map();
        Player player = new Player(0, 0);

        Printing printer = new Printing();
        MenuManager menuManager = new MenuManager();

        while (true)
        {
            printer.Draw(map, player, menuManager);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (menuManager.IsInMenuMode)
                {
                    menuManager.HandleMenuInput(player, map, keyInfo.Key, printer.ShowMessage);
                }
                else
                {
                    char key = keyInfo.KeyChar;
                    if (key == 'e' || key == 'E')
                    {
                        menuManager.ToggleGroundMenu(player, map, printer.ShowMessage);
                    }
                    else if (key == 'b' || key == 'B')
                    {
                        menuManager.ToggleBackpackMenu(player);
                    }
                    else
                    {
                        var (nextX, nextY) = player.NextPosition(key);
                        if (map.IsMovePossible(nextX, nextY))
                            player.Move(nextX, nextY);
                    }
                }
            }
        }
    }
}