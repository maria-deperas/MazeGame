namespace MazeGame.Network.Dto
{
    public class CellDto
    {
        public bool IsWall { get; set; }
        public List<ItemDto> Items { get; set; } = new();
        public EnemyDto? Enemy { get; set; }
    }
}