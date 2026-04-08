using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.UI;
using MazeGame.World;

namespace MazeGame.Commands.Menu
{
    public interface IMenuCommand
    {
        string Description { get; }

        void Execute(MenuManager menu, Player player, Map map, Action<string> showMessage);
    }
}
