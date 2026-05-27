using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items.Weapons;

namespace MazeGame.Items.Weapons
{
    public class RustyMachete : LightWeapon
    {
        public RustyMachete()
        {
            Name = "Rusty Machete";
            Symbol = '~';
            Damage = 25;
        }
    }
}
