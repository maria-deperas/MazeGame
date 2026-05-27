using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using MazeGame.Characters;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    // Jedno aktwne połączenie tcp z konkretnym klientem
    public class ClientSession
    {
        public int Id { get; } // numer gracza
        public Player Player { get; }

        private readonly TcpClient _tcp;
        private readonly NetworkStream _stream; // strumień bajtów do czytania i pisania
        private readonly GameServer _server;

        // kolejka wiadomości czekających na wysłanie do klienta
        private readonly ConcurrentQueue<byte[]> _sendQueue = new();

        public ClientSession(int id, Player player, TcpClient tcp, GameServer server)
        {
            Id = id;
            Player = player;
            _tcp = tcp;
            _stream = tcp.GetStream(); // pobiera strumień z połączenia
            _server = server;
        }

        // uruchamia dwa niezależne wątki w tle dla tego klienta - do czytania i do pisania
        // działają równolegle - klient może równocześnie wysyłać i odbierać
        public void StartAsync()
        {
            Task.Run(ReadLoop);
            Task.Run(WriteLoop);
        }

        // wątek czytający - działa dopóki klient jest połączony
        private async Task ReadLoop()
        {
            try
            {
                using var reader = new StreamReader(_stream, Encoding.UTF8, leaveOpen: true);
                while (_tcp.Connected)
                {
                    string? line = await reader.ReadLineAsync(); // czekaj aż klient coś wyśle
                    if (line == null) break; // zerwanie połączenia

                    var action = JsonProtocol.DeserializeAction(line);
                    if (action != null)
                    {
                        action.PlayerId = Id;
                        _server.ActionQueue.Enqueue((Id, action)); // przekazuje akcje do głównej pętli gry
                    }
                }
            }
            catch {}
            finally
            {
                _server.DisconnectedQueue.Enqueue(Id);
            }
        }

        // wątek piszący
        private async Task WriteLoop()
        {
            try
            {
                while (_tcp.Connected)
                {
                    // wysyła wszystkie wiadomości, które czekają w klejce
                    while (_sendQueue.TryDequeue(out byte[]? data))
                        await _stream.WriteAsync(data, 0, data.Length);

                    await Task.Delay(1);
                }
            }
            catch {}
        }

        // wysyłanie danych, serwer wywołuje metodę z głównego wątku - wrzuca do kolejki
        public void Send(byte[] data) => _sendQueue.Enqueue(data);
    }
}
