using System.Drawing;
using MazeGame.Characters.Enemies.Behaviors;
using MazeGame.Logging;

namespace MazeGame.Characters.Enemies;

public class Skeleton : Enemy
{
    public Skeleton(EnemyStats stats)
        : base("Skeleton", stats, 's', Color.LightGray)
    {
        // agresywny
        PlayerBehavior = new FollowPlayerBehavior();
        SoundBehavior  = new FollowSoundBehavior();
    }

    public override void UpdateOnDeath()
    {
        Stats.Attack += 3;
        Stats.Armor += 2;
        GameLog.GetInstance().Log($"{Name} at [{X},{Y}] rattles with rage! Stats increased.");
    }
}