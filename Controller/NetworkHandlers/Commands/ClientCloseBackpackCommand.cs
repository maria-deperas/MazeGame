namespace MazeGame.Network;

public class ClientCloseBackpackCommand : IClientMenuCommand
{
    public string Description => "Close";
    public void Execute(ClientInputContext ctx) => ctx.IsInBackpackMode = false;
}