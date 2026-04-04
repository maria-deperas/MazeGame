using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Characters.Combat
{
    public interface ICombatVisitor
    {
        string AttackName { get; }
        int CalculateDamageHeavy(int weaponDamage, Attributes stats);
        int CalculateDamageLight(int weaponDamage, Attributes stats);
        int CalculateDamageMagic(int weaponDamage, Attributes stats);
        int CalculateDamageNonWeapon();

        int CalculateDefenseHeavy(Attributes stats);
        int CalculateDefenseLight(Attributes stats);
        int CalculateDefenseMagic(Attributes stats);
        int CalculateDefenseNonWeapon(Attributes stats);
    }
}
