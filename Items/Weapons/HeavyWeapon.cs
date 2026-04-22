using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters.Combat;
using MazeGame.Characters;

namespace MazeGame.Items.Weapons
{
    public abstract class HeavyWeapon : Weapon
    {
        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} (Heavy Weapon, {grip}, DMG: {Damage})";
        }

        public override int AcceptDamage(ICombatVisitor visitor, Attributes stats)
        {
            return visitor.CalculateDamageHeavy(this.Damage, stats);
        }

        public override int AcceptDefense(ICombatVisitor visitor, Attributes stats)
            => visitor.CalculateDefenseHeavy(stats);
    }
}
