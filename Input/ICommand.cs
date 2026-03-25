using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.World;

namespace MazeGame.Input
{
    public interface ICommand
    {
        void Execute();
    }
}
