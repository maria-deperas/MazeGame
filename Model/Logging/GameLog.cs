using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Logging
{
    public class GameLog
    {
        private static GameLog? _instance;
        private readonly List<ILogger> _loggers = new List<ILogger>();

        public MemoryLogger? Memory { get; private set; }

        private GameLog() { }

        public static GameLog GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameLog();
            }
            return _instance;
        }

        public void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void SetMemoryLogger(MemoryLogger memoryLogger)
        {
            Memory = memoryLogger;
            _loggers.Add(memoryLogger);
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Write(message);
            }
        }
    }
}
