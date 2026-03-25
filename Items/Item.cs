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

namespace MazeGame.Items
{
    // bazowa klasa przedmiotu
    public abstract class Item
    {
        public string Name { get; set; } = string.Empty;
        public char Symbol { get; set; } // wyświetlany symbol na mapie
        public abstract Color ItemColor { get; } // kolor danego przedmiotu
        public virtual bool IsTwoHanded => false; // domyślnie przedmioty są jednoręczne
        public virtual bool IsEquippable { get { return false; } }
        public abstract string GetInfo(); // podstawowe informacje o przedmiocie
        public abstract bool PickUp(Player player); // podnoszenie przedmiotu
        public string GetColoredSymbol()
        {
            return Symbol.ToString().Pastel(ItemColor);
        }

        // metody dodawania przedmiotu do prawej/lewej ręki
        public virtual bool AddToRight(Player player)
        {
            Item? InHand = player.RightHand;

            if (InHand != null && !player.CanFitInBackpack(InHand))
                return false;

            if (InHand != null)
            {
                player.AddToBackpack(InHand);
                player.RightHand = null;
            }

            player.RemoveFromBackpack(this);
            player.RightHand = this;
            return true;
        }

        public virtual bool AddToLeft(Player player)
        {
            Item? InHand = player.LeftHand;

            if (InHand != null && !player.CanFitInBackpack(InHand))
                return false;

            if (InHand != null)
            {
                player.AddToBackpack(InHand);
                player.LeftHand = null;
            }

            player.RemoveFromBackpack(this);
            player.LeftHand = this;
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
    }

    public abstract class Weapon : Item
    {
        public int Damage { get; set; }
        public override Color ItemColor => Color.DeepSkyBlue;
        public override bool IsEquippable { get { return true; } }

        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }
    }

    public abstract class OneHandedWeapon : Weapon
    {
        public override string GetInfo() => $"{Name} (one-handed)";

        public override bool AddToRight(Player player)
        {
            if (player.RightHand != null && player.RightHand.IsTwoHanded)
            {
                int availableSpace = Constants.BackpackCapacity - player.Backpack.Count + 1;
                if (availableSpace < 1) return false;

                player.AddToBackpack(player.RightHand);
                player.RightHand = null;
            }
            return base.AddToRight(player);
        }

        public override bool AddToLeft(Player player)
        {
            if (player.RightHand != null && player.RightHand.IsTwoHanded)
            {
                int availableSpace = Constants.BackpackCapacity - player.Backpack.Count + 1;
                if (availableSpace < 1) return false;

                player.AddToBackpack(player.RightHand);
                player.RightHand = null;
            }
            return base.AddToLeft(player);
        }
    }

    public abstract class TwoHandedWeapon : Weapon
    {
        public override bool IsTwoHanded => true;

        public override string GetInfo() => $"{Name} (two-handed)";

        // niezależnie, czy dodamy broń do lewej czy prawej ręki zajmie ona obie ręce
        public override bool AddToLeft(Player player) => AddToBothHands(player);
        public override bool AddToRight(Player player) => AddToBothHands(player);

        private bool AddToBothHands(Player player)
        {
            int itemsInHands = 0;
            if (player.RightHand != null) itemsInHands++;
            if (player.LeftHand != null) itemsInHands++;

            int availableSpace = Constants.BackpackCapacity - player.Backpack.Count + 1;

            // przedmioty znajdujące się aktualnie w rękach muszą mieć miejsce w plecaku
            if (itemsInHands > availableSpace) return false;

            if (player.RightHand != null)
            {
                player.AddToBackpack(player.RightHand);
                player.RightHand = null;
            }

            if (player.LeftHand != null)
            {
                player.AddToBackpack(player.LeftHand);
                player.LeftHand = null;
            }

            player.RemoveFromBackpack(this);
            player.RightHand = this;
            return true;
        }
    }

    public abstract class Currency : Item
    {
        public int Amount { get; protected set; }
        public override Color ItemColor => Color.Gold;

        // Currency ląduje od razu w collected, nie zajmuje miejsca w plecaku/rękach
        public override bool AddToLeft(Player player) => false;
        public override bool AddToRight(Player player) => false;
    }

    public abstract class Unusable : Item
    {
        public override Color ItemColor => Color.ForestGreen;
        public override string GetInfo() => $"{Name} (Unusable)";

        public override bool PickUp(Player player)
        {
            return player.AddToBackpack(this);
        }
    }
    public class Saber : OneHandedWeapon
    {
        public Saber()
        {
            Name = "Saber";
            Symbol = '/';
            Damage = 12;
        }
    }

    public class Dagger : OneHandedWeapon
    {
        public Dagger()
        {
            Name = "Dagger";
            Symbol = '-';
            Damage = 5;
        }
    }

    public class Sword : TwoHandedWeapon
    {
        public Sword()
        {
            Name = "Sword";
            Symbol = 'T';
            Damage = 25;
        }
    }

    public class Gold : Currency
    {
        public override string GetInfo() => $"{Name} (Gold - {Amount})";

        public override bool PickUp(Player player)
        {
            player.Gold += Amount;
            return true;
        }

        public Gold(int amount)
        {
            Name = "Gold Bar";
            Symbol = '▬';
            Amount = amount;
        }
    }

    public class Coin : Currency
    {
        public override string GetInfo() => $"{Name} (Coins - {Amount})";

        public override bool PickUp(Player player)
        {
            player.Coins += Amount;
            return true;
        }

        public Coin(int amount)
        {
            Name = "Silver coin";
            Symbol = '⦿';
            Amount = amount;
        }
    }

    public class Rope : Unusable
    {
        public Rope()
        {
            Name = "Old rope";
            Symbol = 'S';
        }
    }

    public class OldMap : Unusable
    {
        public OldMap()
        {
            Name = "Torn map";
            Symbol = 'm';
        }
    }

    public class Key : Unusable
    {
        public Key()
        {
            Name = "Rusty Key";
            Symbol = 'k';
        }
    }
}

