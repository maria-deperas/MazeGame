using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public class EmptyArenaDirector : MapDirector
    {
        public override Map Generate(IMapBuilder builder)
        {
            return builder
                .BuildEmptyDungeon()
                //.AddRandomWeapons(30)
                .GetResult();
        }
    }
}
