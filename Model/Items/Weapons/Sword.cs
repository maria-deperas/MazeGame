using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Items.Slots;

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
            MaxSlots = 2;
            StatBonus = new Attributes(2, 0, 0, 0, 0, 0);
            Slots = new List<ISlotContent?>(new ISlotContent?[MaxSlots]);
        }
    }
}
