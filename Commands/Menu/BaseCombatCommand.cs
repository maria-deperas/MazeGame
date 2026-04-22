using System;
using MazeGame.Characters;
using MazeGame.Characters.Combat;
using MazeGame.Logging;
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
                playerDamage += player.RightHand.AcceptDamage(visitor, player.Stats);
                playerDefense = player.RightHand.AcceptDefense(visitor, player.Stats);
                holdsAnyWeapon = true;
            }
            if (player.LeftHand != null)
            {
                playerDamage += player.LeftHand.AcceptDamage(visitor, player.Stats);
                playerDefense = player.LeftHand.AcceptDefense(visitor, player.Stats);
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

            GameLog.GetInstance().Log($"Hit {enemy.Name} for {actualDamageToEnemy} dmg!");

            // sprawdzamy czy przeciwnik zginął
            if (enemy.Stats.CurrentHealth <= 0)
            {
                GameLog.GetInstance().Log($"{enemy.Name} died!");
                menu.ActiveCell.Enemy = null;
                menu.CloseMenu();
                return;
            }

            // kontratak przeciwnika
            int actualDamageToPlayer = Math.Max(0, enemy.Stats.Attack - playerDefense);
            player.Stats.Health -= actualDamageToPlayer;

            GameLog.GetInstance().Log($"{enemy.Name} counter-attacks for {actualDamageToPlayer} dmg!");

            // śmierć gracza
            if (player.Stats.Health <= 0)
            {
                GameLog.GetInstance().Log("YOU DIED! GAME OVER.");
                menu.CloseMenu();
            }
        }
    }
}