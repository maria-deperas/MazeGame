namespace MazeGame.Commands
{
    public interface IInputHandler
    {
        IInputHandler SetNext(IInputHandler handler);
        IInputHandler? GetNext();
        bool Handle(ConsoleKey key);
        List<string> GetGuiInstructions();
    }

    public abstract class BaseInputHandler : IInputHandler
    {
        private IInputHandler? _nextHandler;

        public IInputHandler SetNext(IInputHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public IInputHandler? GetNext() => _nextHandler;

        public virtual bool Handle(ConsoleKey key) => _nextHandler?.Handle(key) ?? false;

        public virtual List<string> GetGuiInstructions() => new List<string>();

        protected static string FormatKey(ConsoleKey key) => key switch
        {
            ConsoleKey.UpArrow    => "↑",
            ConsoleKey.DownArrow  => "↓",
            _                     => key.ToString()
        };
    }
}