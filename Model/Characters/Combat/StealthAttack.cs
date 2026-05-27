using MazeGame.Characters;

namespace MazeGame.Characters.Combat
{
    // Skryty atak
    public class StealthAttack : ICombatVisitor
    {
        public string AttackName => "Stealth Attack";

        // Obrażenia
        public int CalculateDamageHeavy(int weaponDamage, Attributes stats) => (weaponDamage + stats.Strength + stats.Aggression) / 2;
        public int CalculateDamageLight(int weaponDamage, Attributes stats) => (weaponDamage + stats.Agility + stats.Luck) * 2;
        public int CalculateDamageMagic(int weaponDamage, Attributes stats) => 1;
        public int CalculateDamageNonWeapon() => 0;

        // Obrona
        public int CalculateDefenseHeavy(Attributes stats) => stats.Strength;
        public int CalculateDefenseLight(Attributes stats) => stats.Agility;
        public int CalculateDefenseMagic(Attributes stats) => 0;
        public int CalculateDefenseNonWeapon(Attributes stats) => 0;
    }
}