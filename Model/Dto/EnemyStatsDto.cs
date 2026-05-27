namespace MazeGame.Network.Dto
{
    public class EnemyStatsDto
    {
        public int MaxHealth     { get; set; }
        public int CurrentHealth { get; set; }
        public int Attack        { get; set; }
        public int Armor         { get; set; }
    }
}