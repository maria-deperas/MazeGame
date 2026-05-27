namespace MazeGame.Network.Dto
{
    public class GameStateDto
    {
        public List<List<CellDto>> Grid { get; set; } = new();
        public List<PlayerDto> Players { get; set; } = new();
        public int LocalPlayerId { get; set; }  // id gracza, któremu wysyłamy ten stan
        public EnemyDto? ActiveEnemy { get; set; } // wróg, z którym lokalny gracz aktualnie walczy
        public List<string> RecentLogs { get; set; } = new();
    }
}