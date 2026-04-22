using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            builder.SetEnemyNames(_themeFactory.GetEnemyNames());
            builder.SetItemPool(_themeFactory.GetItemPool());

            IMapStrategy strategy = _themeFactory.CreateMapStrategy();
            strategy.GenerateMap(builder);

            builder.AddSpecificItem(_themeFactory.CreateArtifact());
            builder.AddInstruction(_themeFactory.GetWelcomeMessage());

            return builder.GetResult();
        }
    }
}
