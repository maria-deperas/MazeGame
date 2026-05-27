using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.World.Generation;

namespace MazeGame.World.Directors
{
    public interface IMapStrategy
    {
        void GenerateMap(IMapBuilder builder);
    }
}