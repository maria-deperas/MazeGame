using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Logging
{
    public class MemoryLogger : ILogger
    {
        private readonly List<(DateTime Time, string Message)> _history = new();

        public int Count => _history.Count;

        public void Write(string message)
        {
            _history.Add((DateTime.Now, message));
        }

        public List<string> GetFullHistory()
        {
            return _history.Select(e => e.Message).ToList();
        }

        public List<string> GetRecentLogs(int count)
        {
            int skip = Math.Max(0, _history.Count - count);
            return _history.Skip(skip).Select(e => e.Message).ToList();
        }

        public List<string> GetRecentLogsWithTimestamps(int count)
        {
            int skip = Math.Max(0, _history.Count - count);
            return _history.Skip(skip)
                           .Select(e => $"[{e.Time:HH:mm:ss}] {e.Message}")
                           .ToList();
        }
    }
}
