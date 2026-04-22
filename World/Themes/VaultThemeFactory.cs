using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items;
using MazeGame.Items.Currency;
using MazeGame.Items.Unusables;
using MazeGame.Items.Weapons;
using MazeGame.World.Directors;

namespace MazeGame.World.Themes
{
    public class VaultThemeFactory : IThemeFactory
    {
        public IMapStrategy CreateMapStrategy()
        {
            return new VaultMapStrategy();
        }

        public Item CreateArtifact()
        {
            return new LuckyCoinBag();
        }

        public string GetWelcomeMessage()
        {
            return "Your pockets feel heavy just looking at this place...";
        }

        public List<string> GetEnemyNames()
        {
            return new List<string> { "Animated Safe", "Gold Golem", "Greedy Spirit" };
        }

        public List<Func<Item>> GetItemPool()
        {
            return new List<Func<Item>>
            {
                () => new Coin(15),
                () => new Coin(25),
                () => new Gold(100),
                () => new Key()
            };
        }
    }
}