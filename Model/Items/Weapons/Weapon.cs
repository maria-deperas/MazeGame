using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.Items.Weapons
{
    public abstract class Weapon : Item
    {
        public virtual int Damage { get; set; }
        public override Color ItemColor => Color.DeepSkyBlue;
        public override bool IsEquippable { get { return true; } }

        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }
        public override bool AddToRight(Player player)
        {
            if (this.IsTwoHanded) return AddToBothHands(player);

            if (player.RightHand != null) return false;

            return base.AddToRight(player);
        }

        public override bool AddToLeft(Player player)
        {
            if(player.LeftHand != null) return false;
            return base.AddToLeft(player);
        }

        private bool AddToBothHands(Player player)
        {
            if (player.RightHand != null || player.LeftHand != null) return false;

            player.RemoveFromBackpack(this);
            player.RightHand = this; 
            return true;
        }
    }
}
