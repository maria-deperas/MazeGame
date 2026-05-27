using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public class LibraryMapStrategy : IMapStrategy
    {
        public void GenerateMap(IMapBuilder builder)
        {
            builder.BuildFilledDungeon()
                   .AddCorridors()
                   .AddRooms(5)
                   .AddRandomLoot(25)
                   .AddEnemies(10);
        }
    }
}