using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items.Weapons;
using MazeGame.Items.Unusables;
using MazeGame.Items.Currency;
using MazeGame.Items.Weapons.Decorators;
using MazeGame.Items.Passives;
using MazeGame.Items.Holders;

namespace MazeGame.Items
{
    // klasa odpowiedzialna za generowanie przedmiotów
    public class ItemPlacer
    {
        // generator liczb losowych do wybierania przedmiotów z listy
        private static Random rng = new Random();

        // listy generatorów nieużywalnych przedmiotów
        private List<Func<Item>> _unusableGenerators = new List<Func<Item>>()
        {
            () => new Rope(),
            () => new OldMap(),
            () => new Key()
        };

        // lista generatorów broni
        private List<Func<Weapon>> _weaponGenerators = new List<Func<Weapon>>()
        {
            () => new Dagger(),
            () => new Saber(),
            () => new Sword(),
            () => new MagicWand()
        };
        
        // lista z generatorami pasywnych 
        private List<Func<Item>> _slotItemGenerators = new List<Func<Item>>()
        {
            () => new LuckStone(),
            () => new StrengthGem(),
            () => new WisdomCrystal(),
            () => new Holder("Small Holder", 2),
            () => new Holder("Large Holder", 4),
        };

        // metoda zwracająca losowy nieużywalny przedmiot
        public Item GetRandomUnusable()
        {
            int index = rng.Next(_unusableGenerators.Count);
            return _unusableGenerators[index]();
        }

        // metoda zwracająca losową broń
        public Item GetRandomWeapon()
        {
            int index = rng.Next(_weaponGenerators.Count);

            Weapon weapon = _weaponGenerators[index]();

            if (rng.Next(100) < 30) 
            {
                weapon = new StrongDecorator(weapon);
            }

            if (rng.Next(100) < 30)
            {
                weapon = new UnluckyDecorator(weapon);
            }

            return weapon;
        }
        
        // zwraca losowy slotitem
        public Item GetRandomSlotItem()
        {
            int index = rng.Next(_slotItemGenerators.Count);
            return _slotItemGenerators[index]();
        }
    }
}
