using MazeGame.Characters;

namespace MazeGame.Items.Passives
{
    public class LuckStone : PassiveItem
    {
        public LuckStone()
        {
            Name = "Luck Stone";
            Symbol = '*';
        }

        public override Attributes StatBonus => new Attributes(0, 0, 0, 2, 0, 0);
    }
}