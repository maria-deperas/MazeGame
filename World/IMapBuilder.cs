using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.World
{
    // Interfejs dla budowniczego mapy - definiuje metody do tworzenia różnych elementów mapy i zwracania gotowej mapy
    public interface IMapBuilder
    {
        IMapBuilder BuildEmptyDungeon();
        IMapBuilder BuildFilledDungeon();
        IMapBuilder AddCorridors();
        IMapBuilder AddRooms(int n);
        IMapBuilder AddCentralRoom(int width, int height);
        IMapBuilder AddRandomItems(int count);
        IMapBuilder AddRandomWeapons(int count);
        Map GetResult();
    }
}
