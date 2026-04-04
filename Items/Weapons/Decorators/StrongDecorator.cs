using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons.Decorators
{
    public class StrongDecorator : WeaponDecorator
    {
        public StrongDecorator(Weapon weaponToWrap) : base(weaponToWrap) { }

        public override string Name => $"{_wrappedWeapon.Name} (Strong)";

        public override int Damage => _wrappedWeapon.Damage + 5;

        public override string GetInfo()
        {
            string grip = IsTwoHanded ? "two-handed" : "one-handed";
            return $"{Name} ({grip}, DMG: {Damage})";
        }
    }
}
