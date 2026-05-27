using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Core;
using MazeGame.Logging;

namespace MazeGame.UI.Panels
{
    public class LogPanel : FramedPanel
    {
        public LogPanel() : base(Constants.RightPanelWidth, 5, "EVENT LOG") { }

        protected override List<string> GetContentLines()
        {
            List<string> lines = new List<string>();
            var recentLogs = GameLog.GetInstance().Memory?.GetRecentLogs(5) ?? new List<string>();

            foreach (var log in recentLogs)
            {
                lines.Add(" " + log);
            }

            return lines;
        }
    }
}
