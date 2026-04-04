using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters.Combat;
using MazeGame.Characters;

namespace MazeGame.Items.Weapons
{
    public abstract class LightWeapon : Weapon
    {
        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} (Light Weapon, {grip}, DMG: {Damage})";
        }
        public override int AcceptDamage(ICombatVisitor visitor, int currentDamage, Attributes stats)
            => visitor.CalculateDamageLight(currentDamage, stats);

        public override int AcceptDefense(ICombatVisitor visitor, Attributes stats)
            => visitor.CalculateDefenseLight(stats);
    }
}