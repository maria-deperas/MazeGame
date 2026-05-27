using MazeGame.Network.Dto;

namespace MazeGame.Network;

public class ClientUnequipRightCommand : IClientMenuCommand
{
    public string Description => "Unequip right";
    public void Execute(ClientInputContext ctx) =>
        ctx.PendingAction = new ActionMessageDto { Type = ActionType.UnequipRight };
}