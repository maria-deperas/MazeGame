using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;

namespace MazeGame.Items.Weapons.Decorators
{
    public class UnluckyDecorator : WeaponDecorator
    {
        public UnluckyDecorator(Weapon weaponToWrap) : base(weaponToWrap) { }
        public override string Name => $"{_wrappedWeapon.Name} (Unlucky)";

        public override bool AddToRight(Player player)
        {
            bool success = base.AddToRight(player);

            if (success)
            {
                player.Stats.Luck -= 5;
            }
            return success;
        }
        public override bool AddToLeft(Player player)
        {
            bool success = base.AddToLeft(player);
            if (success)
            {
                player.Stats.Luck -= 5;
            }
            return success;
        }

        public override bool RemoveFromRight(Player player)
        {
            bool success = base.RemoveFromRight(player);
            if (success)
            {
                player.Stats.Luck += 5;
            }
            return success;
        }
        public override bool RemoveFromLeft(Player player)
        {
            bool success = base.RemoveFromLeft(player);
            if (success)
            {
                player.Stats.Luck += 5;
            }
            return success;
        }


        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} ({grip}, DMG: {Damage})";
        }
    }
}
