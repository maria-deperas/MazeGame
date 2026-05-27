using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    // Abstrakcyjna klasa reprezentująca generację mapy
    public abstract class MapDirector
    {
        public abstract Map Generate(IMapBuilder builder);
    }
}
