using System.Drawing;
using MazeGame.Characters.Enemies.Behaviors;
using MazeGame.Logging;

namespace MazeGame.Characters.Enemies
{
    // neutralny przeciwnik
    public class Troll : Enemy
    {
        public Troll(EnemyStats stats)
            : base("Troll", stats, 't', Color.DarkOliveGreen)
        {
            PlayerBehavior = new IgnoreBehavior();
            SoundBehavior  = new IgnoreBehavior();
        }

        // wywoływane po ataku
        public override void OnAttacked()
        {
            double hpPercent = (double)Stats.CurrentHealth / Stats.MaxHealth;

            if (hpPercent >= 0.5)
            {
                PlayerBehavior = new FollowPlayerBehavior();
                SoundBehavior  = new FollowSoundBehavior();
                GameLog.GetInstance().Log($"{Name} becomes enraged!");
            }
            else
            {
                PlayerBehavior = new FleePlayerBehavior();
                SoundBehavior  = new FleeSoundBehavior();
                GameLog.GetInstance().Log($"{Name} panics and tries to flee!");
            }
        }

        public override void UpdateOnDeath()
        {
            GameLog.GetInstance().Log($"{Name} falls with a thunderous crash.");
        }
    }
}