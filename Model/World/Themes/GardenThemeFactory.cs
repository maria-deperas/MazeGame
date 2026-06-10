using System;
using System.Collections.Generic;
using MazeGame.Items;
using MazeGame.Items.Unusables;
using MazeGame.Items.Weapons;
using MazeGame.World.Directors;

namespace MazeGame.World.Themes
{
    public class GardenThemeFactory : IThemeFactory
    {
        public IMapStrategy CreateMapStrategy()
        {
            return new GardenMapStrategy();
        }

        public Item CreateArtifact()
        {
            return new RustyMachete();
        }

        public string GetWelcomeMessage()
        {
            return "Vines crawl along the walls, and a sweet, sickly scent lingers...";
        }

        public List<string> GetEnemyNames()
        {
            return new List<string> { "Carnivorous Plant", "Corrupted Ent", "Poisonous Slime" };
        }

        public List<Func<Item>> GetItemPool()
        {
            return new List<Func<Item>>
            {
                () => new Rope(),
                () => new Saber(),
                () => new Dagger(),
                //() => new Sword(),
                () => new RustyMachete(),
                () => new MagicWand()
            };
        }
    }
}