using System.Collections.Generic;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;

namespace MazeGame.UI.Panels
{
    public class AttributesPanel : FramedPanel
    {
        private readonly Player _player;
        public AttributesPanel(Player player) : base(Constants.LeftPanelWidth, 4, "ATTRIBUTES")
        { _player = player; }

        protected override List<string> GetContentLines() => new List<string>
        {
            $" Pos: [{_player.X}, {_player.Y}]".PadRight(Width - 2),
            $" Str: {_player.Stats.Strength,-4} Agi: {_player.Stats.Agility}",
            $" HP:  {_player.Stats.Health,-4} Lck: {_player.Stats.Luck}",
            $" Agr: {_player.Stats.Aggression,-4} Wis: {_player.Stats.Wisdom}"
        };
    }
}