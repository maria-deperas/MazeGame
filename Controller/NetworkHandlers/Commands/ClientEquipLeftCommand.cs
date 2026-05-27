using MazeGame.Network.Dto;

namespace MazeGame.Network;

public class ClientEquipLeftCommand : IClientMenuCommand
{
    public string Description => "Left hand";
    public void Execute(ClientInputContext ctx)
    {
        ctx.PendingAction = new ActionMessageDto { Type = ActionType.EquipLeft, SelectionIndex = ctx.SelectionIndex };
        ctx.IsInBackpackMode = false;
    }
}