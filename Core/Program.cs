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
        InitializeConsole();

        // konfiguracja i Logowanie
        GameConfig config = LoadConfig("config.json");
        SetupLogging(config);

        // budowanie Świata
        Map map = CreateMap();
        Player player = new Player(0, 0, config.PlayerName);

        // przygotowanie interfejsu
        MenuManager menuManager = new MenuManager();
        var (leftColumn, mapRenderer, rightColumn, menuHandler) = InitializeUI(player, map, menuManager);

        // start gry
        InstructionScreen welcomeScreen = new InstructionScreen();
        welcomeScreen.ShowAndAwaitStart(map.Instructions);

        RunGameLoop(player, menuHandler, leftColumn, mapRenderer, rightColumn);
    }

    private static void InitializeConsole()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
    }

    private static void SetupLogging(GameConfig config)
    {
        GameLog gameLog = GameLog.GetInstance();
        gameLog.AddLogger(new FileLogger(config.LogFilePath, config.PlayerName));
        gameLog.SetMemoryLogger(new MemoryLogger());
        gameLog.Log($"{config.PlayerName} enters the labyrinth.");
    }

    private static Map CreateMap()
    {
        IMapBuilder builder = new StandardMapBuilder();
        var factory = new MazeGame.World.Themes.GardenThemeFactory();
        var strategy = new ThemeDungeonDirector(factory);
        return strategy.Generate(builder);
    }

    private static (IRenderable left, IRenderable mid, IRenderable right, MenuInputHandler handler) InitializeUI(Player player, Map map, MenuManager menuManager)
    {
        int totalWidth = Constants.LeftPanelWidth + Constants.MazeWidth + Constants.RightPanelWidth + 6;
        int totalHeight = Constants.PanelHeight + 5;
        Console.SetWindowSize(totalWidth, totalHeight);
        Console.SetBufferSize(totalWidth, totalHeight);

        Action<string> showMessage = msg => GameLog.GetInstance().Log(msg);

        // chain of responsibility
        var menuHandler = new MenuInputHandler(menuManager, player, map, showMessage);
        var movementHandler = new MovementHandler(player, map, menuManager, showMessage);
        var actionHandler = new ActionHandler(player, map, menuManager, showMessage);
        menuHandler.SetNext(movementHandler).SetNext(actionHandler);

        // Panele
        var left = new VerticalStackPanel(
            Constants.LeftPanelWidth,
            Constants.PanelHeight,
            new AttributesPanel(player),
            new WalletPanel(player),
            new ControlsPanel(menuManager, menuHandler)
        );

        var right = new VerticalStackPanel(
            Constants.RightPanelWidth,
            Constants.PanelHeight,
            new EquippedPanel(player),
            new GroundItemsPanel(map, player, menuManager),
            new BackpackPanel(player, menuManager),
            new CombatPanel(menuManager), new LogPanel()
        );

        return (left, new MapRenderer(map, player), right, menuHandler);
    }

    private static void RunGameLoop(Player player, MenuInputHandler handler, IRenderable left, IRenderable mid, IRenderable right)
    {
        ScreenRenderer renderer = new ScreenRenderer();

        while (true)
        {
            renderer.DrawScreen(left, mid, right);

            if (Console.KeyAvailable)
            {
                if (!handler.Handle(Console.ReadKey(true).Key))
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