using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.UI;

namespace MazeGame.Commands.Exploration
{
    public class OpenLogCommand : ICommand
    {
        public string Description => "Journal";
        public bool IsVisible => true;

        public void Execute()
        {
            LogScreen.Show();
        }
    }
}
