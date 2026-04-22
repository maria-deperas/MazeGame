using System.Text;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.UI.Panels;
using MazeGame.World;
using MazeGame.Core;
using MazeGame.World.Generation;
using MazeGame.World.Directors;
using MazeGame.Commands.Exploration;

using System.IO;
using System.Text.Json;
using MazeGame.Logging;

public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // Wczytywanie konfiguracji z pliku JSON
        GameConfig config = LoadConfig("config.json");

        GameLog gameLog = GameLog.GetInstance();

        var fileLogger = new FileLogger(config.LogFilePath, config.PlayerName);
        var memoryLogger = new MemoryLogger();

        gameLog.AddLogger(fileLogger);
        gameLog.SetMemoryLogger(memoryLogger);
        gameLog.Log($"{config.PlayerName} enters the labyrinth.");

        int totalWidth = Constants.LeftPanelWidth + Constants.MazeWidth + Constants.RightPanelWidth + 6;
        int totalHeight = Constants.PanelHeight + 5;

        Console.SetWindowSize(totalWidth, totalHeight);
        Console.SetBufferSize(totalWidth, totalHeight);

        IMapBuilder builder = new StandardMapBuilder();
        MazeGame.World.Themes.IThemeFactory factory = new MazeGame.World.Themes.GardenThemeFactory();
        MapDirector strategy = new MazeGame.World.Directors.ThemeDungeonDirector(factory);
        Map map = strategy.Generate(builder);

        // Ekran powitalny z instrukcjami
        InstructionScreen welcomeScreen = new InstructionScreen();
        welcomeScreen.ShowAndAwaitStart(map.Instructions);

        // Inicjalizacja gracza i zarządzania menu
        Player player = new Player(0, 0, config.PlayerName);
        MenuManager menuManager = new MenuManager();
        ScreenRenderer screenRenderer = new ScreenRenderer();

        // 2. Podpinamy go pod nasz delegat showMessage
        Action<string> showMessage = msg => GameLog.GetInstance().Log(msg);
        // Lewa kolumna
        var attrPanel = new AttributesPanel(player);
        var walletPanel = new WalletPanel(player);

        var menuHandler = new MenuInputHandler(menuManager, player, map, showMessage);
        var movementHandler = new MovementHandler(player, map, menuManager, showMessage);
        var actionHandler = new ActionHandler(player, map, menuManager, showMessage);

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
        var logPanel = new LogPanel();

        var rightColumn = new VerticalStackPanel(
            Constants.RightPanelWidth,
            Constants.PanelHeight,
            equipPanel, groundPanel, backpackPanel, combatPanel, logPanel
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
                    GameLog.GetInstance().Log("An unknown button was pressed.");
                }
            }

            if (player.Stats.Health <= 0)
            {
                GameOverScreen.Show();
                break;
            }
        }
    }
    // metoda odpowiedzialna za wczytywanie konfiguracji z pliku JSON. 
    static GameConfig LoadConfig(string path)
    {
        // Jeśli plik nie istnieje, tworzy domyślny plik z ustawieniami.
        if (!File.Exists(path))
        {
            var defaultConfig = new GameConfig();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string defaultJson = JsonSerializer.Serialize(defaultConfig, options);
            File.WriteAllText(path, defaultJson);
            return defaultConfig;
        }

        // Jeśli plik istnieje i jest poprawny, wczytuje konfigurację. 
        try
        {
            string jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GameConfig>(jsonString) ?? new GameConfig();
        }
        catch (Exception)
        {
            // Jeśli plik istnieje, ale jest uszkodzony, zwraca domyślne ustawienia bez wywoływania błędu.
            return new GameConfig();
        }
    }
}