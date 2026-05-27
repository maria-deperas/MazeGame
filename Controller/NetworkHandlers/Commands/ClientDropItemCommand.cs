using MazeGame.Network.Dto;

namespace MazeGame.Network;

public class ClientDropItemCommand : IClientMenuCommand
{
    public string Description => "Drop";
    public void Execute(ClientInputContext ctx) =>
        ctx.PendingAction = new ActionMessageDto { Type = ActionType.DropItem, SelectionIndex = ctx.SelectionIndex };
}