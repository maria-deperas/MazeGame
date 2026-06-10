using System.Drawing;
using MazeGame.Characters;
using MazeGame.Items.Slots;

namespace MazeGame.Items.Passives
{
    // leaf kompozytu
    public abstract class PassiveItem : Item, ISlotContent
    {
        public abstract Attributes StatBonus { get; }
        public override Color ItemColor => Color.MediumPurple;
        public override bool IsEquippable => false; // nie można włożyć do ręki, tylko przez slot
        public override int SoundRange => 1;

        // po podniesieniu trafia do plecaka
        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }

        public Attributes GetStatBonus() => StatBonus;

        public string GetSlotInfo() => GetInfo();

        public override string GetInfo()
        {
            return $"{Name} [STR+{StatBonus.Strength} AGI+{StatBonus.Agility} " +
                   $"HP+{StatBonus.Health} LCK+{StatBonus.Luck} " +
                   $"AGR+{StatBonus.Aggression} WIS+{StatBonus.Wisdom}]";
        }
    }
}