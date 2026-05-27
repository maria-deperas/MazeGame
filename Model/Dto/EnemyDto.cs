namespace MazeGame.Network.Dto
{
    public class EnemyDto
    {
        public string? Name { get; set; }
        public char Symbol { get; set; }
        public string? ColorHex { get; set; }  // Kolor w formacie "#RRGGBB"
        public int X { get; set; }
        public int Y { get; set; }
        public EnemyStatsDto? Stats { get; set; }
    }
}