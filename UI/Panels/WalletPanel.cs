using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class WalletPanel : FramedPanel
    {
        private readonly Player _player;
        public WalletPanel(Player player) : base(Constants.LeftPanelWidth, 2, "WALLET")
        { _player = player; }

        protected override List<string> GetContentLines() => new List<string>
        {
            $" Coins: {_player.Coins}",
            $" Gold:  {_player.Gold}"
        };
    }
}