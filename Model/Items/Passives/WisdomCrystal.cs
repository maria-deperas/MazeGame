using MazeGame.Characters;

namespace MazeGame.Items.Passives
{
    public class WisdomCrystal : PassiveItem
    {
        public WisdomCrystal()
        {
            Name = "Wisdom Crystal";
            Symbol = '*';
        }

        public override Attributes StatBonus => new Attributes(0, 0, 0, 0, 0, 2);
    }
}