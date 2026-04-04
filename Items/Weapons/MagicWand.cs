using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters.Combat;
using MazeGame.Characters;

namespace MazeGame.Items.Weapons
{
    public class MagicWand : MagicWeapon
    {
        public MagicWand()
        {
            Name = "Magic Wand";
            Symbol = 'i';
            Damage = 15;
        }

        public override int AcceptDamage(ICombatVisitor visitor, int currentDamage, Attributes stats)
            => visitor.CalculateDamageMagic(currentDamage, stats);

        public override int AcceptDefense(ICombatVisitor visitor, Attributes stats)
            => visitor.CalculateDefenseMagic(stats);
    }
}
