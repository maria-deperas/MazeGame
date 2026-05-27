using System.Text;
using System.IO;
using System.Text.Json;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.UI.Panels;
using MazeGame.World;
using MazeGame.Core;
using MazeGame.World.Generation;
using MazeGame.World.Directors;
using MazeGame.Commands.Exploration;
using MazeGame.Commands;
using MazeGame.Logging;
using MazeGame.Network;

public class Program
{
    static void Main(string[] args)
    {
        string mode = ParseMode(args);

        // tryb klienta sieciowego
        if (mode == "client")
        {
            var (host, port) = ParseClientAddress(args);
            new GameClient(host, port).Run();
            return;
        }

        
        // tryb serwera
        // tworzy model gry bez gracza - serwer nie gra, zarządza stanem
        if (mode == "server")
        {
            int port = ParsePort(args, defaultPort: 5555);
            Map map = CreateMap();
            GameModel model = new GameModel(map, null); // serwer nie ma własnego gracza

            var serverLog = new MemoryLogger();
            GameLog.GetInstance().SetMemoryLogger(serverLog);

            GameServer server = new GameServer(model, port);
            server.Start(); // uruchamia acceptloop w tle

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            RunServerLoop(model, server, serverLog);
            return;
        }

        // tryb lokalny
        InitializeConsole();
        GameConfig config = LoadConfig("config.json");
        SetupLogging(config);

        Map localMap = CreateMap();
        Player player = new Player(0, 0, config.PlayerName);
        GameModel gameModel = new GameModel(localMap, player);

        MenuManager menuManager = new MenuManager(gameModel);
        var (view, menuHandler) = InitializeUI(gameModel, menuManager);

        InstructionScreen welcomeScreen = new InstructionScreen();
        welcomeScreen.ShowAndAwaitStart(gameModel.Map.Instructions);

        RunGameLoop(gameModel, view, menuHandler);
    }

    // główna pętla serwera
    private static void RunServerLoop(GameModel model, GameServer server, MemoryLogger log)
    {
        var serverView = new ServerView(server, log);

        while (true)
        {
            // obsługuje połączenia, rozłączenia i akcje graczy
            bool changed = server.ProcessPendingEvents(); 
            // przesmieszczanie sie wrogow
            if (model.TryTickEnemies(server.GetCombatEnemies()))
            {
                changed = true;
            }

            // jeśli coś się zmieniło wysyła do wszystkich klientów
            if (changed) server.Broadcast();

            // odświeża konsolę serwera
            serverView.Render();
            Thread.Sleep(20);
        }
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

    private static (IGameView view, MenuInputHandler handler) InitializeUI(GameModel gameModel, MenuManager menuManager)
    {
        int totalWidth  = Constants.LeftPanelWidth + Constants.MazeWidth + Constants.RightPanelWidth + 6;
        int totalHeight = Constants.PanelHeight + 5;
        Console.SetWindowSize(totalWidth, totalHeight);
        Console.SetBufferSize(totalWidth, totalHeight);

        Player player = gameModel.Player!;
        Map    map    = gameModel.Map;

        var menuHandler     = new MenuInputHandler(menuManager);
        var movementHandler = new MovementHandler(gameModel);
        var actionHandler   = new ActionHandler(gameModel, () => LogScreen.Show());
        menuHandler.SetNext(movementHandler).SetNext(actionHandler);

        Func<List<string>> explorationInstructions = () =>
        {
            var lines = new List<string>();
            IInputHandler? current = menuHandler;
            while (current != null)
            {
                lines.AddRange(current.GetGuiInstructions());
                current = current.GetNext();
            }
            return lines;
        };

        var left = new VerticalStackPanel(
            Constants.LeftPanelWidth,
            Constants.PanelHeight,
            new AttributesPanel(player),
            new WalletPanel(player),
            new ControlsPanel(gameModel, explorationInstructions, () => menuManager.GetActiveMenuInfo())
        );

        var right = new VerticalStackPanel(
            Constants.RightPanelWidth,
            Constants.PanelHeight,
            new EquippedPanel(player),
            new GroundItemsPanel(map, player, gameModel),
            new BackpackPanel(player, gameModel),
            new CombatPanel(gameModel),
            new LogPanel()
        );

        IGameView view = new ConsoleGameView(left, new MapRenderer(gameModel), right);
        return (view, menuHandler);
    }

    // główna pętla trybu lokalnego
    private static void RunGameLoop(
        GameModel     model,
        IGameView     view,
        IInputHandler handler)
    {
        while (true)
        {
            view.Render();

            if (Console.KeyAvailable)
            {
                if (!handler.Handle(Console.ReadKey(true).Key))
                    GameLog.GetInstance().Log("Nieznany klawisz.");
            }

            model.TryTickEnemies();

            if (model.Player!.Stats.Health <= 0)
            {
                view.ShowGameOver();
                break;
            }
        }
    }

    // tryb uruchomienia gry
    private static string ParseMode(string[] args)
    {
        if (args.Contains("--server")) return "server";
        if (args.Contains("--client")) return "client";

        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Uruchomić jako (S)erwer, (K)lient czy (L)okalnie?");
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.S) return "server";
            if (key == ConsoleKey.K) return "client";
            if (key == ConsoleKey.L || key == ConsoleKey.Enter) return "local";
        }
    }

    // parsuje adres serwera z arg
    private static (string host, int port) ParseClientAddress(string[] args)
    {
        int idx = Array.IndexOf(args, "--client");
        if (idx >= 0 && idx + 1 < args.Length)
        {
            string addr = args[idx + 1];
            var parts   = addr.Split(':');
            string host = parts[0];
            int port    = parts.Length > 1 && int.TryParse(parts[1], out int p) ? p : 5555;
            return (host, port);
        }
        return ("127.0.0.1", 5555);
    }

    private static int ParsePort(string[] args, int defaultPort)
    {
        int idx = Array.IndexOf(args, "--server");
        if (idx >= 0 && idx + 1 < args.Length && int.TryParse(args[idx + 1], out int p))
            return p;
        return defaultPort;
    }

    static GameConfig LoadConfig(string path)
    {
        if (!File.Exists(path))
        {
            var defaultConfig = new GameConfig();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string defaultJson = JsonSerializer.Serialize(defaultConfig, options);
            File.WriteAllText(path, defaultJson);
            return defaultConfig;
        }

        try
        {
            string jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GameConfig>(jsonString) ?? new GameConfig();
        }
        catch (Exception)
        {
            return new GameConfig();
        }
    }
}
