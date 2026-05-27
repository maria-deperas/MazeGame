namespace MazeGame.Network.Dto
{
    public class PlayerDto
    {
        public int PlayerId { get; set; } // numer gracza
        public string? Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public AttributesDto? Stats { get; set; }
        public List<ItemDto> Backpack { get; set; } = new();
        public ItemDto? LeftHand { get; set; }
        public ItemDto? RightHand { get; set; }
        public int Coins { get; set; }
        public int Gold { get; set; }
    }
}