namespace MazeGame.Commands.Exploration
{
    public class OpenLogCommand : ICommand
    {
        private readonly Action _showLog;

        public OpenLogCommand(Action showLog)
        {
            _showLog = showLog;
        }

        public string Description => "Journal";
        public bool IsVisible => true;

        public void Execute() => _showLog();
    }
}