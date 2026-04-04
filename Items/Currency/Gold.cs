using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;

namespace MazeGame.Items.Currency
{
    public class Gold : Currency
    {
        public override string GetInfo() => $"{Name} (Gold - {Amount})";

        public override bool PickUp(Player player)
        {
            player.Gold += Amount;
            return true;
        }

        public Gold(int amount)
        {
            Name = "Gold Bar";
            Symbol = '▬';
            Amount = amount;
        }

    }
}
