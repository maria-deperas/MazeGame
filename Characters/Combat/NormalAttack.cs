using MazeGame.Characters;

namespace MazeGame.Characters.Combat
{
    public class NormalAttack : ICombatVisitor
    {
        public string AttackName => "Normal Attack";

        // --- OBRAŻENIA ---
        public int CalculateDamageHeavy(int weaponDamage, Attributes stats) => weaponDamage + stats.Strength + stats.Aggression; // Bez zmian
        public int CalculateDamageLight(int weaponDamage, Attributes stats) => weaponDamage + stats.Agility + stats.Luck; // Bez zmian
        public int CalculateDamageMagic(int weaponDamage, Attributes stats) => 1; // Magiczna zredukowana do 1
        public int CalculateDamageNonWeapon() => 0; // Nie-broń zadaje 0

        // --- OBRONA ---
        public int CalculateDefenseHeavy(Attributes stats) => stats.Strength + stats.Luck;
        public int CalculateDefenseLight(Attributes stats) => stats.Agility + stats.Luck;
        public int CalculateDefenseMagic(Attributes stats) => stats.Agility + stats.Luck;
        public int CalculateDefenseNonWeapon(Attributes stats) => stats.Agility;
    }
}