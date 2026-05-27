using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Core
{
    public class GameConfig
    {
        public string PlayerName { get; set; } = "Anonymous player";
        public string DungeonTheme { get; set; } = "Standard";
        public string LogFilePath { get; set; } = "logs/";
    }
}