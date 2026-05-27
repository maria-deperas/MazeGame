using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.Core;

namespace MazeGame.UI.Panels
{
    public class EquippedPanel : FramedPanel
    {
        private readonly Player _player;
        public EquippedPanel(Player player) : base(Constants.RightPanelWidth, 2, "EQUIPPED")
        { _player = player; }

        protected override List<string> GetContentLines()
        {
            string r = _player.RightHand != null ? _player.RightHand.GetInfo() : "Empty";
            string l = _player.LeftHand != null ? _player.LeftHand.GetInfo() : "Empty";
            if (_player.RightHand != null && _player.RightHand.IsTwoHanded) l = r;
            if (_player.LeftHand != null && _player.LeftHand.IsTwoHanded) r = l;

            return new List<string> { $" L: {l}", $" R: {r}" };
        }
    }
}