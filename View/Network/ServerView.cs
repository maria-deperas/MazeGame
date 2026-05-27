using MazeGame.Logging;
using MazeGame.Network;

namespace MazeGame.UI
{
    public class ServerView
    {
        private readonly GameServer _server;
        private readonly MemoryLogger _log;
        private int _lastCount    = -1;
        private int _lastLogCount = -1;

        private const int LogLines = 20;

        public ServerView(GameServer server, MemoryLogger log)
        {
            _server = server;
            _log    = log;
        }

        public void Render()
        {
            int count    = _server.ConnectedCount;
            int logCount = _log.Count;

            if (count == _lastCount && logCount == _lastLogCount) return;
            _lastCount    = count;
            _lastLogCount = logCount;

            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       MazeGame  Server           ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine($"  Podłączeni gracze: {count} / 9");
            Console.WriteLine();
            Console.WriteLine(" ───────────── Dziennik zdarzeń ─────────────");

            var logs = _log.GetRecentLogsWithTimestamps(LogLines);
            if (logs.Count == 0)
            {
                Console.WriteLine("  (brak zdarzeń)");
            }
            else
            {
                foreach (var entry in logs)
                    Console.WriteLine("  " + entry);
            }

            Console.WriteLine();
            Console.WriteLine("  Naciśnij Ctrl+C aby zatrzymać serwer.");
        }
    }
}
