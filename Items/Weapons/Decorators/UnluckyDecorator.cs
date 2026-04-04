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

        // Modyfikujemy nazwę!
        public override string Name => $"{_wrappedWeapon.Name} (Unlucky)";

        // Podpinamy się pod moment wzięcia broni do prawej ręki
        public override bool AddToRight(Player player)
        {
            // Najpierw próbujemy wziąć broń normalnie (delegacja)
            bool success = base.AddToRight(player);

            // Jeśli się udało, zabieramy 5 szczęścia
            if (success)
            {
                player.Stats.Luck -= 5;
            }
            return success;
        }

        // Podpinamy się pod moment wzięcia broni do lewej ręki
        public override bool AddToLeft(Player player)
        {
            bool success = base.AddToLeft(player);
            if (success)
            {
                player.Stats.Luck -= 5;
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
