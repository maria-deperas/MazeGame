using MazeGame.Characters;

namespace MazeGame.Items.Passives
{
    public class StrengthGem : PassiveItem
    {
        public StrengthGem()
        {
            Name = "Strength Gem";
            Symbol = '*';
        }

        public override Attributes StatBonus => new Attributes(2, 0, 0, 0, 0, 0);
    }
}