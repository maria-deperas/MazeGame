using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public class VaultMapStrategy : IMapStrategy
    {
        public void GenerateMap(IMapBuilder builder)
        {
            builder.BuildFilledDungeon()
                   .AddCentralRoom(16, 8)
                   .AddCorridors()
                   .AddRandomLoot(35) 
                   .AddEnemies(8);
        }
    }
}