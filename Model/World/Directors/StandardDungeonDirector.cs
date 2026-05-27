using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public class StandardDungeonDirector : MapDirector
    {
        public override Map Generate(IMapBuilder builder)
        {
            return builder
                .BuildFilledDungeon()
                .AddCorridors()
                .AddCentralRoom(10, 4)
                .AddRooms(6)
                //.AddRandomItems(20)
                //.AddRandomWeapons(30)
                .AddEnemies(10)
                .GetResult();
        }
    }
}
