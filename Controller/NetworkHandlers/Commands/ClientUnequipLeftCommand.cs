using MazeGame.Network.Dto;

namespace MazeGame.Network;

public class ClientUnequipLeftCommand : IClientMenuCommand
{
    public string Description => "Unequip left";
    public void Execute(ClientInputContext ctx) =>
        ctx.PendingAction = new ActionMessageDto { Type = ActionType.UnequipLeft };
}