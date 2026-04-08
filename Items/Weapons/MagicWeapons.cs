using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters.Combat;
using MazeGame.Characters;

namespace MazeGame.Items.Weapons
{
    public abstract class MagicWeapon : Weapon
    {
        public override int AcceptDamage(ICombatVisitor visitor, int currentDamage, Attributes stats)
            => visitor.CalculateDamageMagic(currentDamage, stats);

        public override int AcceptDefense(ICombatVisitor visitor, Attributes stats)
            => visitor.CalculateDefenseMagic(stats);
        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} (Magic Weapon, {grip}, DMG: {Damage})";
        }
    }
}
