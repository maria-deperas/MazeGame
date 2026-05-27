using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;

namespace MazeGame.Items.Currency
{
    public abstract class Currency : Item
    {
        public int Amount { get; protected set; }
        public override Color ItemColor => Color.Gold;

        // Currency ląduje od razu w collected, nie zajmuje miejsca w plecaku/rękach
        public override bool AddToLeft(Player player) => false;
        public override bool AddToRight(Player player) => false;
    }
}
