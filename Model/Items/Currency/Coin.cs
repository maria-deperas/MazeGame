using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;

namespace MazeGame.Items.Currency
{
    public class Coin : Currency
    {
        public override string GetInfo() => $"{Name} (Coins - {Amount})";

        public override bool PickUp(Player player)
        {
            player.Coins += Amount;
            return true;
        }

        public Coin(int amount)
        {
            Name = "Silver coin";
            Symbol = '⦿';
            Amount = amount;
        }
    }
}
