using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;

namespace MazeGame.Items.Unusables
{
    public abstract class Unusable : Item
    {
        public override Color ItemColor => Color.ForestGreen;
        public override string GetInfo() => $"{Name} (Unusable)";
        public override bool IsEquippable { get { return true; } }

        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }
    }
}
