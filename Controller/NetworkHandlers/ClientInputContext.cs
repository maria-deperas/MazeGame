using MazeGame.Network.Dto;

namespace MazeGame.Network
{
    public class ClientInputContext
    {
        public int SelectionIndex    { get; set; } = 0;
        public ActionMessageDto? PendingAction { get; set; }
        public bool IsInBackpackMode { get; set; } = false;
        public bool IsInCombat       { get; set; } = false;
        public bool IsLocalPlayerDead { get; set; } = false;
    }
}
