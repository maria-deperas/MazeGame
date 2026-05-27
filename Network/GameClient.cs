using System.Net.Sockets;
using System.Text;
using MazeGame.Commands;
using MazeGame.Core;
using MazeGame.UI;

namespace MazeGame.Network
{
    // Strona klienta - łączy się z serwerem i obsługuje komunikację sieciową
    public class GameClient
    {
        private readonly string _host; // IP serwera
        private readonly int _port; // port serwera
        private TcpClient? _tcp; // połączenie tcp, null - gdy nie jest połączony

        public GameClient(string host = "127.0.0.1", int port = 5555)
        {
            _host = host;
            _port = port;
        }

        public void Run()
        {
            // aktualny stan klienta między handlerem a widokiem
            var ctx = new ClientInputContext();
            
            // chain of responsibility
            IInputHandler handler = new ClientBackpackHandler(ctx);
            handler.SetNext(new ClientMovementHandler(ctx))
                   .SetNext(new ClientSelectionHandler(ctx))
                   .SetNext(new ClientPickUpHandler(ctx))
                   .SetNext(new ClientCombatHandler(ctx));

            // zbiera instrukcje ze wszystkich handlerów, widok wywołuje ją przy renderowaniu, żeby pokazać aktualne opcje
            Func<List<string>> getControlsInstructions = () =>
            {
                var lines = new List<string>();
                IInputHandler? current = handler;
                while (current != null)
                {
                    lines.AddRange(current.GetGuiInstructions());
                    current = current.GetNext();
                }
                return lines;
            };

            var view = new NetworkGameView(ctx, getControlsInstructions);

            // próba nawiązania połączenia z serwerem
            _tcp = new TcpClient();
            try { _tcp.Connect(_host, _port); }
            catch (Exception ex)
            {
                view.ShowConnectionError(_host, _port, ex.Message);
                Console.ReadKey(true);
                return;
            }

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible  = false;
            try
            {
                int totalWidth  = Constants.LeftPanelWidth + Constants.MazeWidth + Constants.RightPanelWidth + 6;
                int totalHeight = Constants.PanelHeight + 8;
                Console.SetWindowSize(totalWidth, totalHeight);
                Console.SetBufferSize(totalWidth, totalHeight);
            }
            catch {}

            Console.Clear();
            view.ShowConnecting();

            var stream = _tcp.GetStream(); // strumień bajtów z komunikacją

            // wątek odbierający - czeka na kolejne linie JSON od serwera
            // deserializuje stan gry, aktualizuje widok i ctx
            // osobny wątek -> ReadLineAsync blokuje
            Task.Run(async () =>
            {
                using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                while (_tcp.Connected)
                {
                    string? line = await reader.ReadLineAsync(); // czeka na dane od serwera
                    if (line == null) break;
                    var state = JsonProtocol.DeserializeState(line);
                    if (state != null)
                    {
                        view.UpdateState(state); // odświeża dane widoku
                        ctx.IsInCombat = state.ActiveEnemy != null; // aktualizuje stan walki
                        var me = state.Players.FirstOrDefault(p => p.PlayerId == state.LocalPlayerId);
                        ctx.IsLocalPlayerDead = me?.Stats?.Health <= 0; // wykrywa śmierć gracza
                    }
                }
            });

            // writer do wysyłania akcji
            using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

            // główna pętla klienta
            while (_tcp.Connected)
            {
                view.Render();

                // śmierć gracza
                if (ctx.IsLocalPlayerDead)
                {
                    _tcp.Close(); // zamyka połączenie
                    view.ShowGameOver();
                    return;
                }

                // obsługa klawiatury
                if (Console.KeyAvailable)
                {
                    ctx.PendingAction = null;
                    handler.Handle(Console.ReadKey(true).Key); // przetwarzanie klawisza
                    if (ctx.PendingAction != null)
                        writer.WriteLine(JsonProtocol.Serialize(ctx.PendingAction)); // wysyła JSON do serwera
                }

                Thread.Sleep(20);
            }

            // gdy serwer zamknie połączenie
            view.ShowDisconnected();
            Console.ReadKey(true);
        }
    }
}
