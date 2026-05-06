using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Pastel;
using System.Drawing;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;
using MazeGame.Characters.Combat;

namespace MazeGame.Items
{
    public abstract class Item
    {
        public virtual string Name { get; set; } = string.Empty;
        public virtual char Symbol { get; set; } // wyświetlany symbol na mapie
        public abstract Color ItemColor { get; } // kolor danego przedmiotu
        public virtual bool IsTwoHanded => false; // domyślnie przedmioty są jednoręczne
        public virtual bool IsEquippable => false;
        public virtual int SoundRange => 1;
        public abstract string GetInfo(); // podstawowe informacje o przedmiocie
        public abstract bool PickUp(Player player); // podnoszenie przedmiotu
        public string GetColoredSymbol()
        {
            return Symbol.ToString().Pastel(ItemColor);
        }

        // metody dodawania przedmiotu do prawej/lewej ręki
        public virtual bool AddToRight(Player player)
        {
            if (player.RightHand != null) return false;

            player.RemoveFromBackpack(this);
            player.RightHand = this;
            return true;
        }

        public virtual bool AddToLeft(Player player)
        {
            if (player.LeftHand != null) return false;
            if (player.RightHand != null && player.RightHand.IsTwoHanded) return false;

            player.RemoveFromBackpack(this);
            player.LeftHand = this;
            return true;
        }
        public virtual bool RemoveFromRight(Player player)
        {
            if (player.RightHand != this) return false;
            if (!player.CanFitInBackpack(this)) return false; // Brak miejsca w plecaku

            player.RightHand = null;
            player.AddToBackpack(this);
            return true;
        }

        public virtual bool RemoveFromLeft(Player player)
        {
            if (player.LeftHand != this) return false;
            if (!player.CanFitInBackpack(this)) return false; // Brak miejsca w plecaku

            player.LeftHand = null;
            player.AddToBackpack(this);
            return true;
        }

        // metoda wyrzucająca przedmiot z plecaka
        public virtual bool Drop(Player player, Cell currentCell)
        {
            if (currentCell.Items.Count >= Constants.MaxItemsPerCell) return false;
            player.RemoveFromBackpack(this);
            currentCell.Items.Add(this);
            return true;
        }

        public virtual int AcceptDamage(ICombatVisitor visitor, Attributes stats)
        {
            return visitor.CalculateDamageNonWeapon();
        }

        public virtual int AcceptDefense(ICombatVisitor visitor, Attributes stats)
        {
            return visitor.CalculateDefenseNonWeapon(stats);
        }
    }
}

