using System.Text;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.UI.Panels;
using MazeGame.World;
using MazeGame.Core;
using MazeGame.World.Generation;
using MazeGame.World.Directors;
using MazeGame.Commands.Exploration;

public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        int totalWidth = Constants.LeftPanelWidth + Constants.MazeWidth + Constants.RightPanelWidth + 6;
        int totalHeight = Constants.PanelHeight + 5;

        Console.SetWindowSize(totalWidth, totalHeight);
        Console.SetBufferSize(totalWidth, totalHeight);

        IMapBuilder builder = new StandardMapBuilder();
        MapDirector strategy = new StandardDungeonDirector();
        Map map = strategy.Generate(builder);

        // Ekran powitalny z instrukcjami
        InstructionScreen welcomeScreen = new InstructionScreen();
        welcomeScreen.ShowAndAwaitStart(map.Instructions);

        // Inicjalizacja gracza i zarządzania menu
        Player player = new Player(0, 0);
        MenuManager menuManager = new MenuManager();
        ScreenRenderer screenRenderer = new ScreenRenderer();

        // Lewa kolumna
        var attrPanel = new AttributesPanel(player);
        var walletPanel = new WalletPanel(player);

        var menuHandler = new MenuInputHandler(menuManager, player, map, screenRenderer.ShowMessage);
        var movementHandler = new MovementHandler(player, map, menuManager, screenRenderer.ShowMessage);
        var actionHandler = new ActionHandler(player, map, menuManager, screenRenderer.ShowMessage);

        menuHandler.SetNext(movementHandler).SetNext(actionHandler);

        var controlsPanel = new ControlsPanel(menuManager, menuHandler);

        var leftColumn = new VerticalStackPanel(
            Constants.LeftPanelWidth,
            Constants.PanelHeight,
            attrPanel, walletPanel, controlsPanel
        );

        // Prawa kolumna
        var equipPanel = new EquippedPanel(player);
        var groundPanel = new GroundItemsPanel(map, player, menuManager);
        var backpackPanel = new BackpackPanel(player, menuManager);
        var combatPanel = new CombatPanel(menuManager);

        var rightColumn = new VerticalStackPanel(
            Constants.RightPanelWidth,
            Constants.PanelHeight,
            equipPanel, groundPanel, backpackPanel, combatPanel
        );

        var mapRenderer = new MapRenderer(map, player);

        // Główna pętla gry
        while (true)
        {
            screenRenderer.DrawScreen(leftColumn, mapRenderer, rightColumn);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                bool handled = menuHandler.Handle(keyInfo.Key);

                if (!handled)
                {
                    screenRenderer.ShowMessage("Unknown key!");
                }
            }

            if (player.Stats.Health <= 0)
            {
                GameOverScreen.Show();
                break;
            }
        }
    }
}