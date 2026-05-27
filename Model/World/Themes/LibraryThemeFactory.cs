using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items.Unusables;
using MazeGame.Items.Weapons;
using MazeGame.Items;
using MazeGame.World.Directors;

namespace MazeGame.World.Themes
{
    public class LibraryThemeFactory : IThemeFactory
    {
        public IMapStrategy CreateMapStrategy()
        {
            return new LibraryMapStrategy();
        }

        // unikalny artefakt 
        public Item CreateArtifact()
        {
            return new MagicBook();
        }

        // wiadomość powitalna
        public string GetWelcomeMessage()
        {
            return "The smell of old paper and dust fills the air...";
        }

        // lista przeciwników, którzy mogą się pojawić w lochu
        public List<string> GetEnemyNames()
        {
            return new List<string> { "Dark Mage", "Animated Tome", "Ghostly Librarian" };
        }

        // przedmioty, które możą się pojawić w lochu
        public List<Func<Item>> GetItemPool()
        {
            return new List<Func<Item>>
            {
                () => new OldMap(),
                () => new MagicWand(),
                () => new Dagger()
            };
        }
    }
}
