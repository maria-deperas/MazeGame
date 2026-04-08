using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Characters.Combat;

namespace MazeGame.Items.Weapons.Decorators
{
    // Abstrakcyjna klasa dekoratora dla broni, implementująca wzorzec dekoratora
    public abstract class WeaponDecorator : Weapon
    {
        protected Weapon _wrappedWeapon;

        public WeaponDecorator(Weapon weaponToWrap)
        {
            _wrappedWeapon = weaponToWrap;
        }
        public override string Name
        {
            get => _wrappedWeapon.Name;
            set => _wrappedWeapon.Name = value;
        }

        public override char Symbol
        {
            get => _wrappedWeapon.Symbol;
            set => _wrappedWeapon.Symbol = value;
        }

        public override int Damage
        {
            get => _wrappedWeapon.Damage;
            set => _wrappedWeapon.Damage = value;
        }

        public override Color ItemColor => _wrappedWeapon.ItemColor;
        public override bool IsEquippable => _wrappedWeapon.IsEquippable;
        public override bool IsTwoHanded => _wrappedWeapon.IsTwoHanded;

        public override int AcceptDamage(ICombatVisitor visitor, int currentDamage, Attributes stats)
        {
            return _wrappedWeapon.AcceptDamage(visitor, currentDamage, stats);
        }

        public override int AcceptDefense(ICombatVisitor visitor, Attributes stats)
        {
            return _wrappedWeapon.AcceptDefense(visitor, stats);
        }

    }
}
