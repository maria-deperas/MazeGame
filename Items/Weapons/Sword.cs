using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons
{
    public class Sword : HeavyWeapon
    {
        public override bool IsTwoHanded => true;
        public Sword()
        {
            Name = "Sword";
            Symbol = 'T';
            Damage = 25;
        }
    }
}
