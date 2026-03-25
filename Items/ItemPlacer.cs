using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private List<Func<Item>> _weaponGenerators = new List<Func<Item>>()
        {
            () => new Dagger(),
            () => new Saber(),
            () => new Sword()
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
            return _weaponGenerators[index]();
        }
    }
}
