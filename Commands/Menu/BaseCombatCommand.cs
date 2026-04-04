using System;
using System.ComponentModel.Design;
using MazeGame.Characters;
using MazeGame.Characters.Combat;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public abstract class BaseCombatCommand : IMenuAction
    {
        public abstract string Description { get; }

        // Ta metoda wymusi na dziedziczących klasach dostarczenie odpowiedniego Odwiedzającego
        protected abstract ICombatVisitor GetVisitor();

        public void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage)
        {
            Enemy enemy = menu.ActiveEnemy;
            if (enemy == null) return;

            ICombatVisitor visitor = GetVisitor();

            // --- 1. TURA GRACZA ---
            int playerDamage = 0;
            int playerDefense = 0;

            // Zakładamy bicie z prawej ręki (możesz to potem rozbudować)
            if (player.RightHand != null)
            {
                // Rzutowanie na ogólny typ Weapon jest legalne w instrukcji i służy tylko pobraniu surowej statystyki Damage
                int baseDmg = (player.RightHand as Items.Weapons.Weapon)?.Damage ?? 0;

                // TU DZIEJE SIĘ MAGIA WZORCA VISITOR! 0 if-ów!
                playerDamage = player.RightHand.AcceptDamage(visitor, baseDmg, player.Stats);
                playerDefense = player.RightHand.AcceptDefense(visitor, player.Stats);
            }
            else
            {
                playerDamage = visitor.CalculateDamageNonWeapon();
                playerDefense = visitor.CalculateDefenseNonWeapon(player.Stats);
            }

            int actualDamageToEnemy = Math.Max(0, playerDamage - enemy.Stats.Armor);
            enemy.Stats.CurrentHealth -= actualDamageToEnemy;

            string log = $"[{visitor.AttackName}] You hit {enemy.Name} for {actualDamageToEnemy} dmg!";

            if (enemy.Stats.CurrentHealth <= 0)
            {
                log += $" {enemy.Name} died! You won!";
                menu.ActiveCell.Enemy = null; // Wróg znika z mapy
                menu.CloseMenu(); // Koniec walki
                showMessage(log);
                return; // Przerywamy, bo trup nie oddaje
            }

            // --- 2. TURA WROGA (Kontratak) ---
            int actualDamageToPlayer = Math.Max(0, enemy.Stats.Attack - playerDefense);
            player.Stats.Health -= actualDamageToPlayer;

            log += $"\n{enemy.Name} counter-attacks for {actualDamageToPlayer} dmg! (Your HP: {player.Stats.Health})";

            if (player.Stats.Health <= 0)
            {
                log += "\nYOU DIED! GAME OVER.";
                menu.CloseMenu();
                // Tu w przyszłości dodasz np. Environment.Exit(0) by zamknąć grę
            }
            else
            {
                // Walka trwa dalej, odświeżamy zachętę
                log += $"\nChoose next attack: [1] Normal, [2] Stealth, [3] Magic";
            }

            showMessage(log);
        }
    }
}