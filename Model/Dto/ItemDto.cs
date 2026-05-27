namespace MazeGame.Network.Dto
{
    public class ItemDto
    {
        public string? Name { get; set; }
        public char Symbol { get; set; }
        public int Damage { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsTwoHanded { get; set; }
        public string? Info { get; set; }
    }
}