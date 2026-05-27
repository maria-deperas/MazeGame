using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons
{
    public class Saber : LightWeapon
    {
        public Saber()
        {
            Name = "Saber";
            Symbol = '/';
            Damage = 12;
        }
    }
}
