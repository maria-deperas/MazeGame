using System.Drawing;
using MazeGame.Characters;
using MazeGame.World;

namespace MazeGame.Items
{
    public interface IItem
    {
        string Name { get; set; }
        char Symbol { get; set; }
        Color ItemColor { get; }
        bool IsTwoHanded { get; }
        bool IsEquippable { get; }
        int SoundRange { get; }

        string GetInfo();
        string GetColoredSymbol();
        bool PickUp(Player player);
        bool AddToRight(Player player);
        bool AddToLeft(Player player);
        bool Drop(Player player, Cell currentCell);
    }
}