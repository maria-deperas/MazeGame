using System;
using MazeGame.Characters;
using MazeGame.Characters.Combat;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public abstract class BaseCombatCommand : IMenuCommand
    {
        public abstract string Description { get; }
        protected abstract ICombatVisitor GetVisitor();

        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            Enemy enemy = menu.ActiveEnemy;
            if (enemy == null) return;

            ICombatVisitor visitor = GetVisitor();

            int playerDamage = 0;
            int playerDefense = 0;
            bool holdsAnyWeapon = false;

            // Obliczanie obrażeń i obrony na podstawie broni w rękach
            if (player.RightHand != null)
            {
                int rightBaseDmg = (player.RightHand as Items.Weapons.Weapon)?.Damage ?? 0;
                playerDamage += player.RightHand.AcceptDamage(visitor, rightBaseDmg, player.Stats);
                playerDefense += player.RightHand.AcceptDefense(visitor, player.Stats);
                holdsAnyWeapon = true;
            }
            if (player.LeftHand != null)
            {
                int leftBaseDmg = (player.LeftHand as Items.Weapons.Weapon)?.Damage ?? 0;
                playerDamage += player.LeftHand.AcceptDamage(visitor, leftBaseDmg, player.Stats);
                playerDefense += player.LeftHand.AcceptDefense(visitor, player.Stats);
                holdsAnyWeapon = true;
            }
            if (!holdsAnyWeapon)
            {
                playerDamage = visitor.CalculateDamageNonWeapon();
                playerDefense = visitor.CalculateDefenseNonWeapon(player.Stats);
            }

            // Atakujemy przeciwnika
            int actualDamageToEnemy = Math.Max(0, playerDamage - enemy.Stats.Armor);
            enemy.Stats.CurrentHealth -= actualDamageToEnemy;

            string log = $"[{visitor.AttackName}] You hit {enemy.Name} for {actualDamageToEnemy} dmg!";

            // Gdy przeciwnik ginie usuwamy go z mapy i zamykamy menu walki
            if (enemy.Stats.CurrentHealth <= 0)
            {
                log += $" {enemy.Name} died! You won!";
                menu.ActiveCell.Enemy = null;
                menu.CloseMenu();
                showMessage(log);
                return;
            }

            // Kontratak przeciwnika
            int actualDamageToPlayer = Math.Max(0, enemy.Stats.Attack - playerDefense);
            player.Stats.Health -= actualDamageToPlayer;

            log += $"\n{enemy.Name} counter-attacks for {actualDamageToPlayer} dmg! (Your HP: {player.Stats.Health})";

            // Jeśli gracz ginie, zamykamy menu walki i wyświetlamy komunikat o śmierci
            if (player.Stats.Health <= 0)
            {
                log += "\nYOU DIED! GAME OVER.";
                menu.CloseMenu();
            }
            showMessage(log);
        }
    }
}