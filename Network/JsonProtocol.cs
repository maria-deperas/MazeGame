using System.Text;
using System.Text.Json;
using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    // metody pomocnicze do serializacji i deserializacji wiadomości sieciowych
    public static class JsonProtocol
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        // serializacja

        // stan gry -> striing JSON
        public static string Serialize(GameStateDto state)
            => JsonSerializer.Serialize(state, Options);

        // akcja gracza -> string JSON
        public static string Serialize(ActionMessageDto action)
            => JsonSerializer.Serialize(action, Options);

        //deserializacja

        public static GameStateDto? DeserializeState(string json)
            => JsonSerializer.Deserialize<GameStateDto>(json, Options);

        public static ActionMessageDto? DeserializeAction(string json)
            => JsonSerializer.Deserialize<ActionMessageDto>(json, Options);

        // kodowanie do bajtów (do wysyłania przez TcpClient)
        public static byte[] Encode(string message)
            => Encoding.UTF8.GetBytes(message + "\n");

        // do broadcast()
        public static byte[] EncodeState(GameStateDto state)
            => Encode(Serialize(state));

        // do wysyłania akcji
        public static byte[] EncodeAction(ActionMessageDto action)
            => Encode(Serialize(action));
    }
}