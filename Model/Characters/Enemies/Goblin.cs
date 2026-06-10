using System.Drawing;
using MazeGame.Characters.Enemies.Behaviors;
using MazeGame.Logging;

namespace MazeGame.Characters.Enemies;

public class Goblin : Enemy
{
    public Goblin(EnemyStats stats)
        : base("Goblin", stats, 'g', Color.Green)
    {
        // tchórzliwy
        PlayerBehavior = new FleePlayerBehavior();
        SoundBehavior  = new FleeSoundBehavior();
    }

    public override void UpdateOnDeath()
    {
        Stats.Attack = Math.Max(1, Stats.Attack - 2);
        Stats.Armor = Math.Max(0, Stats.Armor - 1);
        GameLog.GetInstance().Log($"{Name} at [{X},{Y}] trembles in fear! Stats decreased.");
    }
}