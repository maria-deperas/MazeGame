using MazeGame.Network.Dto;

namespace MazeGame.Network;

public class ClientEquipRightCommand : IClientMenuCommand
{
    public string Description => "Right hand";
    public void Execute(ClientInputContext ctx)
    {
        ctx.PendingAction = new ActionMessageDto { Type = ActionType.EquipRight, SelectionIndex = ctx.SelectionIndex };
        ctx.IsInBackpackMode = false;
    }
}