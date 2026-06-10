using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items.Holders;
using MazeGame.Items.Passives;
using MazeGame.Items.Weapons;
using MazeGame.World.Generation;
using MazeGame.World.Themes;

namespace MazeGame.World.Directors
{
    public class ThemeDungeonDirector : MapDirector
    {
        private readonly IThemeFactory _themeFactory;

        public ThemeDungeonDirector(IThemeFactory factory)
        {
            _themeFactory = factory;
        }

        public override Map Generate(IMapBuilder builder)
        {
            var demoSword = new Sword();

            var innerHolder = new Holder("Inner Holder", 2);
            var deepHolder  = new Holder("Deep Holder",  2);

            deepHolder.InsertItem(1, new WisdomCrystal());
            innerHolder.InsertItem(0, new StrengthGem());
            innerHolder.InsertItem(1, deepHolder);
            demoSword.InsertIntoSlot(0, new LuckStone());
            demoSword.InsertIntoSlot(1, innerHolder);

            builder.SetEnemyNames(_themeFactory.GetEnemyNames());
            builder.SetItemPool(_themeFactory.GetItemPool());

            IMapStrategy strategy = _themeFactory.CreateMapStrategy();
            strategy.GenerateMap(builder);

            builder.AddSpecificItem(_themeFactory.CreateArtifact());
            builder.AddSpecificItem(demoSword);
            builder.AddSpecificItem(new Holder("Small Holder", 2));
            builder.AddSpecificItem(new Holder("Large Holder", 4));
            builder.AddRandomSlotItems(5);
            builder.AddInstruction(_themeFactory.GetWelcomeMessage());

            return builder.GetResult();
        }
    }
}
