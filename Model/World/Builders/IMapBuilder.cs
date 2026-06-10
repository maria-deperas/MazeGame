using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Items;

namespace MazeGame.World.Generation
{
    // Interfejs dla budowniczego mapy - definiuje metody do tworzenia różnych elementów mapy i zwracania gotowej mapy
    public interface IMapBuilder
    {
        IMapBuilder BuildEmptyDungeon();
        IMapBuilder BuildFilledDungeon();
        IMapBuilder AddCorridors();
        IMapBuilder AddRooms(int n);
        IMapBuilder AddCentralRoom(int width, int height);
        IMapBuilder SetEnemyNames(List<string> names);
        IMapBuilder SetItemPool(List<Func<Item>> pool);
        IMapBuilder AddSpecificItem(Item item);
        IMapBuilder AddInstruction(string message);
        IMapBuilder AddRandomLoot(int count);
        IMapBuilder AddEnemies(int count);
        IMapBuilder AddRandomSlotItems(int count);
        Map GetResult();
    }
}
