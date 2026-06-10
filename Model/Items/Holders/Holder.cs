using System.Collections.Generic;
using System.Drawing;
using MazeGame.Characters;
using MazeGame.Items.Slots;

namespace MazeGame.Items.Holders
{
    // composite kompozytu, implementuje ISlotContent
    public class Holder : Item, ISlotContent
    {
        public int MaxSlots { get; }
        public List<ISlotContent?> Slots { get; } // do przechowywania innych ISlotContent

        public Holder(string name, int maxSlots)
        {
            Name = name;
            Symbol = 'u';
            MaxSlots = maxSlots;
            Slots = new List<ISlotContent?>(new ISlotContent?[maxSlots]);
        }

        public override Color ItemColor => Color.SaddleBrown;
        public override bool IsEquippable => false;

        public override bool PickUp(Player player) => player.AddToBackpack(this);

        // wywołanie GetStatBonus na każdym elemencie w swoich slotach
        public Attributes GetStatBonus()
        {
            var total = new Attributes(0, 0, 0, 0, 0, 0);
            foreach (var item in Slots)
                if (item != null) total.Add(item.GetStatBonus());
            return total;
        }

        // wstawia ISlotContent do slotu o podanym indeksie
        public bool InsertItem(int slotIndex, ISlotContent item)
        {
            if (slotIndex < 0 || slotIndex >= MaxSlots) return false;
            if (Slots[slotIndex] != null) return false;
            Slots[slotIndex] = item;
            return true;
        }

        // wyjmuje i zwraca element ze slotu
        public ISlotContent? RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxSlots) return null;
            var removed = Slots[slotIndex];
            Slots[slotIndex] = null;
            return removed;
        }

        public string GetSlotInfo() => GetInfo();

        public override string GetInfo() =>
            $"{Name} (Holder, {MaxSlots} slots, bonus: {GetStatBonus().Strength}STR " +
            $"{GetStatBonus().Luck}LCK {GetStatBonus().Wisdom}WIS)";
    }
}