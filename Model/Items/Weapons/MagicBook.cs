using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items.Weapons;

namespace MazeGame.Items.Weapons
{
    public class MagicBook : MagicWeapon
    {
        public MagicBook()
        {
            Name = "Magic Book";
            Symbol = 'B';
            Damage = 30;
        }
    }
}
