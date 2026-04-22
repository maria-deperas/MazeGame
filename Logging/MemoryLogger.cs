using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Logging
{
    public class MemoryLogger : ILogger
    {
        private readonly List<string> _history = new List<string>();

        public void Write(string message)
        {
            _history.Add(message);
        }

        public List<string> GetFullHistory()
        {
            return new List<string>(_history);
        }

        public List<string> GetRecentLogs(int count)
        {
            int skip = Math.Max(0, _history.Count - count);
            return _history.Skip(skip).ToList();
        }
    }
}
