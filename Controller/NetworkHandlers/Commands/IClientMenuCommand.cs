namespace MazeGame.Network
{
    public interface IClientMenuCommand
    {
        string Description { get; }
        void Execute(ClientInputContext ctx);
    }
}