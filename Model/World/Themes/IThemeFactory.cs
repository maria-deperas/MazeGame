using System;
using System.Collections.Generic;
using MazeGame.Items;
using MazeGame.World.Directors;

namespace MazeGame.World.Themes
{
    public interface IThemeFactory
    {
        IMapStrategy CreateMapStrategy();
        Item CreateArtifact();
        string GetWelcomeMessage();
        List<string> GetEnemyNames();
        List<Func<Item>> GetItemPool();
    }
}