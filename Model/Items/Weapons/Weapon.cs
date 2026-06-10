using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.Items.Slots;

namespace MazeGame.Items.Weapons
{
    public abstract class Weapon : Item
    {
        public virtual int Damage { get; set; }
        public override Color ItemColor => Color.DeepSkyBlue;
        public override bool IsEquippable { get { return true; } }
        public int MaxSlots { get; protected set; } = 0; // ile slotów ma broń
        public virtual Attributes StatBonus { get; protected set; } = new Attributes(0, 0, 0, 0, 0, 0);
        // lista slotów danej broni
        public List<ISlotContent?> Slots { get; protected set; } = new List<ISlotContent?>();

        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }
        public override bool AddToRight(Player player)
        {
            bool success;
            if (this.IsTwoHanded)
                success = AddToBothHands(player);
            else
            {
                if (player.RightHand != null) return false;
                success = base.AddToRight(player);
            }
            if (success) { player.Stats.Add(GetSlotBonus()); 
                player.Stats.Add(StatBonus); }
            return success;
        }

        public override bool AddToLeft(Player player)
        {
            if (player.LeftHand != null) return false;
            bool success = base.AddToLeft(player);
            if (success) { player.Stats.Add(GetSlotBonus()); 
                player.Stats.Add(StatBonus); }
            return success;
        }

        public override bool RemoveFromRight(Player player)
        {
            var bonus = GetSlotBonus();
            bool success = base.RemoveFromRight(player);
            if (success) { player.Stats.Subtract(bonus); 
                player.Stats.Subtract(StatBonus); }
            return success;
        }

        public override bool RemoveFromLeft(Player player)
        {
            var bonus = GetSlotBonus();
            bool success = base.RemoveFromRight(player);
            if (success) { player.Stats.Subtract(bonus); 
                player.Stats.Subtract(StatBonus); }
            return success;
        }

        private bool AddToBothHands(Player player)
        {
            if (player.RightHand != null || player.LeftHand != null) return false;

            player.RemoveFromBackpack(this);
            player.RightHand = this; 
            return true;
        }
        
        // iteruje po slotach broni i rekurencyjnie sumuje bonusy
        public Attributes GetSlotBonus()
        {
            var total = new Attributes(0, 0, 0, 0, 0, 0);
            foreach (var item in Slots)
                if (item != null) total.Add(item.GetStatBonus());
            return total;
        }

        public bool InsertIntoSlot(int slotIndex, ISlotContent item)
        {
            if (slotIndex < 0 || slotIndex >= MaxSlots) return false;
            if (Slots[slotIndex] != null) return false;
            Slots[slotIndex] = item;
            return true;
        }

        public ISlotContent? RemoveFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxSlots) return null;
            var removed = Slots[slotIndex];
            Slots[slotIndex] = null;
            return removed;
        }
    }
}
