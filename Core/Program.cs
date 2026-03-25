using System;
using System.Text;
using MazeGame;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;
using MazeGame.Input;

public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        IMapBuilder builder = new StandardMapBuilder();
        MapGeneration strategy = new FirstMap();
        Map map = strategy.Generate(builder);

        // Ekran powitalny z instrukcjami
        InstructionScreen welcomeScreen = new InstructionScreen();
        welcomeScreen.ShowAndAwaitStart(map.Instructions);

        // Inicjalizacja gracza, systemu rysowania i zarządzania menu
        Player player = new Player(0, 0);
        Printing printer = new Printing();
        MenuManager menuManager = new MenuManager();

        // Ustawienie chain of responsibility dla obsługi wejścia z klawiatury
        var menuHandler = new MenuInputHandler(menuManager, player, map, printer.ShowMessage);
        var movementHandler = new MovementHandler(player, map);
        var actionHandler = new ActionHandler(player, map, menuManager, printer.ShowMessage);

        menuHandler.SetNext(movementHandler).SetNext(actionHandler);

        // Główna pętla gry
        while (true)
        {
            printer.Draw(map, player, menuManager);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);


                bool handled = menuHandler.Handle(keyInfo.Key);

                if (!handled)
                {
                    printer.ShowMessage("Nieznany klawisz! Użyj W/A/S/D, E lub B.");
                }
            }
        }
    }
}