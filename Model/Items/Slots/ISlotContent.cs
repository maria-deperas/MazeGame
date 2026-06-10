using MazeGame.Characters;

namespace MazeGame.Items.Slots
{
    // component kompozytu
    // - wspólny interfejs dla wszystkich rzeczy, które mogą trafić do slotu
    // PassiveItem i Holder go implementują
    public interface ISlotContent
    {
        string Name { get; }
        Attributes GetStatBonus(); // zwraca atrybuty z bonusami
        string GetSlotInfo(); // opis
    }
}