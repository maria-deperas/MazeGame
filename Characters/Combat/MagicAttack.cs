using MazeGame.Characters;

namespace MazeGame.Characters.Combat
{
    public class MagicAttack : ICombatVisitor
    {
        public string AttackName => "Magic Attack";

        // --- OBRAŻENIA ---
        public int CalculateDamageHeavy(int weaponDamage, Attributes stats) => 1;
        public int CalculateDamageLight(int weaponDamage, Attributes stats) => 1;
        public int CalculateDamageMagic(int weaponDamage, Attributes stats) => weaponDamage + stats.Wisdom;
        public int CalculateDamageNonWeapon() => 0;

        // --- OBRONA ---
        public int CalculateDefenseHeavy(Attributes stats) => stats.Luck;
        public int CalculateDefenseLight(Attributes stats) => stats.Luck;

        // UWAGA: Upewnij się, że w Twojej klasie Attributes ta właściwość nazywa się Wisdom (albo zmień tu na np. Intelligence)
        public int CalculateDefenseMagic(Attributes stats) => stats.Wisdom * 2;

        public int CalculateDefenseNonWeapon(Attributes stats) => stats.Luck;
    }
}