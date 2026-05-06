using System.Drawing;
using MazeGame.Logging;

namespace MazeGame.Characters.Enemies;

public class Skeleton : Enemy
{
    public Skeleton(EnemyStats stats) 
        : base("Skeleton", stats, 's', Color.LightGray) { }

    public override void UpdateOnDeath()
    {
        Stats.Attack += 3;
        Stats.Armor += 2;
        GameLog.GetInstance().Log($"{Name} at [{X},{Y}] rattles with rage! Stats increased.");
    }

    public override void UpdateOnSound(int sourceX, int sourceY, int range)
    {
        GameLog.GetInstance().Log($"{Name} at [{X},{Y}] heard a noise from [{sourceX},{sourceY}].");
    }
}