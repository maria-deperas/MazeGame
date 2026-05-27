using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Items.Weapons
{
    public class LuckyCoinBag : HeavyWeapon
    {
        public override bool IsTwoHanded => true;
        public LuckyCoinBag()
        {
            Name = "Lucky Coin Bag";
            Symbol = '$';
            Damage = 35;
        }
    }
}
