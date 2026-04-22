using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string directory, string playerName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _filePath = Path.Combine(directory, $"{playerName}_{timestamp}.txt");
        }

        public void Write(string message)
        {
            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}\n";
            File.AppendAllText(_filePath, logEntry);
        }

        public string GetFilePath() => _filePath;
    }
}
