using MazeGame.Characters;

namespace MazeGame.Characters.Combat
{
    // Atak magiczny
    public class MagicAttack : ICombatVisitor
    {
        public string AttackName => "Magic Attack";

        // Obrażenia
        public int CalculateDamageHeavy(int weaponDamage, Attributes stats) => 1;
        public int CalculateDamageLight(int weaponDamage, Attributes stats) => 1;
        public int CalculateDamageMagic(int weaponDamage, Attributes stats) => weaponDamage + stats.Wisdom;
        public int CalculateDamageNonWeapon() => 0;

        // Obrona
        public int CalculateDefenseHeavy(Attributes stats) => stats.Luck;
        public int CalculateDefenseLight(Attributes stats) => stats.Luck;
        public int CalculateDefenseMagic(Attributes stats) => stats.Wisdom * 2;
        public int CalculateDefenseNonWeapon(Attributes stats) => stats.Luck;
    }
}