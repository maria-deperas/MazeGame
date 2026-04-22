using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public class GardenMapStrategy : IMapStrategy
    {
        public void GenerateMap(IMapBuilder builder)
        {
            builder.BuildFilledDungeon()
                   .AddRooms(15)
                   .AddCorridors()
                   .AddRandomLoot(20)
                   .AddEnemies(15); 
        }
    }
}