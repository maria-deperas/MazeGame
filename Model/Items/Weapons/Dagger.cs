using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons
{
    public class Dagger : LightWeapon
    {
        public Dagger()
        {
            Name = "Dagger";
            Symbol = '-';
            Damage = 5;
        }
    }

}
