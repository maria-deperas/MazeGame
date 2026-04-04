using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons
{
    public abstract class MagicWeapon : Weapon
    {
        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} (Magic Weapon, {grip}, DMG: {Damage})";
        }
    }
}
