using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.World
{
    // Abstrakcyjna klasa reprezentująca generację mapy
    public abstract class MapGeneration
    {
        public abstract Map Generate(IMapBuilder builder);
    }

    // Konkretny generator mapy - tworzy mapę wypełnioną korytarzami, pokojami i przedmiotami
    public class FirstMap : MapGeneration
    {
        public override Map Generate(IMapBuilder builder)
        {
            return builder
                .BuildFilledDungeon()
                .AddCorridors()
                .AddCentralRoom(10, 4)
                .AddRooms(6)
                .AddRandomItems(20)
                .AddRandomWeapons(30)
                .GetResult();
        }
    }

    // Inny generator mapy - tworzy pustą mapę z losowymi broniami
    public class SecondMap : MapGeneration
    {
        public override Map Generate(IMapBuilder builder)
        {
            return builder
                .BuildEmptyDungeon()
                .AddRandomWeapons(30)
                .GetResult();
        }
    }
}
