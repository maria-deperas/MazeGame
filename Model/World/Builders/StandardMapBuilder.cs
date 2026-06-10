using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.Items;
using MazeGame.Characters.Enemies;
using MazeGame.Events;
using MazeGame.Items.Passives;
using MazeGame.Items.Holders;

namespace MazeGame.World.Generation
{
    // Builder mapy
    public class StandardMapBuilder : IMapBuilder
    {
        private Cell[,]? _cells;
        private Random _rnd = new Random();
        private ItemPlacer _itemFactory = new ItemPlacer();

        private List<string> _instructions = new List<string>();

        private List<string> _enemyNames = new List<string> { "Default enemy" };
        private List<Func<Item>> _itemPool = new List<Func<Item>>();
        
        private SoundPublisher _soundPublisher = new SoundPublisher();
        private Dictionary<string, SpeciesPublisher> _speciesPublishers = new Dictionary<string, SpeciesPublisher>();

        // Metoda pomocnicza do sprawdzania, czy mapa została zainicjalizowana
        private void CheckInitialized()
        {
            if (_cells == null)
                throw new InvalidOperationException("Initialize map first!");
        }

        // Metoda do budowania pustego lochu
        public IMapBuilder BuildEmptyDungeon()
        {
            _cells = new Cell[Constants.MazeHeight, Constants.MazeWidth];
            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    _cells[y, x] = new Cell { IsWall = false };
                }
            }
            return this;
        }

        // Metoda do budowania wypełnionego lochu
        public IMapBuilder BuildFilledDungeon()
        {
            _cells = new Cell[Constants.MazeHeight, Constants.MazeWidth];
            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                for (int x = 0; x < Constants.MazeWidth; x++)
                {
                    _cells[y, x] = new Cell { IsWall = true };
                }
            }
            return this;
        }

        // Metoda pomocnicza do dodawania nagłówka tylko raz, gdy dodajemy pierwszą strukturę
        private void EnsureStructureHeader()
        {
            if (!_instructions.Contains("MAP STRUCTURE:"))
            {
                _instructions.Add("MAP STRUCTURE:");
            }
        }

        // Metoda do dodawania centralnego pokoju
        public IMapBuilder AddCentralRoom(int width, int height)
        {
            CheckInitialized();
            int startX = (Constants.MazeWidth - width) / 2;
            int startY = (Constants.MazeHeight - height) / 2;

            for (int y = startY; y < startY + height; y++)
                for (int x = startX; x < startX + width; x++)
                    _cells![y, x].IsWall = false;

            EnsureStructureHeader();
            if (!_instructions.Any(s => s.Contains("Central room")))
            {
                _instructions.Add($" - Central room: A massive, {width}x{height} main chamber in the heart of the dungeon.");
            }

            return this;
        }

        // Metoda do dodawania korytarzy za pomocą zrandomizowanego algorytmu Prima
        public IMapBuilder AddCorridors()
        {
            CheckInitialized();

            List<(int x, int y)> possiblesteps = new List<(int, int)>();

            // Pozycja startowa
            int startX = 0;
            int startY = 0;

            _cells![startY, startX].IsWall = false;

            // Możliwe kierunki ruchu (skok o 2, bo między nimi jest ściana)
            (int dx, int dy)[] directions = { (0, -2), (2, 0), (0, 2), (-2, 0) };

            // Funkcja do dodawania nowych możliwych kroków do listy
            void AddPossibleSteps(int x, int y)
            {
                foreach (var dir in directions)
                {
                    int nx = x + dir.dx;
                    int ny = y + dir.dy;

                    if (nx >= 0 && nx < Constants.MazeWidth && ny >= 0 && ny < Constants.MazeHeight)
                    {
                        if (_cells[ny, nx].IsWall && !possiblesteps.Contains((nx, ny)))
                        {
                            possiblesteps.Add((nx, ny));
                        }
                    }
                }
            }

            AddPossibleSteps(startX, startY);

            // Główna pętla algorytmu Prima
            while (possiblesteps.Count > 0)
            {
                // Losowo wybieramy jedną z możliwych komórek
                int index = _rnd.Next(possiblesteps.Count);
                var current = possiblesteps[index];
                possiblesteps.RemoveAt(index);

                var visitedNeighbors = new List<(int nx, int ny, int wallX, int wallY)>();
                foreach (var dir in directions)
                {
                    int nx = current.x + dir.dx;
                    int ny = current.y + dir.dy;

                    // Sprawdzamy, czy sąsiad jest w granicach i czy jest już odwiedzony
                    if (nx >= 0 && nx < Constants.MazeWidth && ny >= 0 && ny < Constants.MazeHeight)
                    {
                        if (!_cells[ny, nx].IsWall)
                        {
                            visitedNeighbors.Add((nx, ny, current.x + dir.dx / 2, current.y + dir.dy / 2));
                        }
                    }
                }

                // Jeśli mamy sąsiadów, łączymy aktualną komórkę z losowo wybranym sąsiadem
                if (visitedNeighbors.Count > 0)
                {
                    var neighbor = visitedNeighbors[_rnd.Next(visitedNeighbors.Count)];

                    // Usuwamy ścianę między aktualną komórką a sąsiadem
                    _cells[neighbor.wallY, neighbor.wallX].IsWall = false;

                    // Odwiedzamy aktualną komórkę
                    _cells[current.y, current.x].IsWall = false;

                    AddPossibleSteps(current.x, current.y);
                }
            }

            EnsureStructureHeader();
            if (!_instructions.Any(s => s.Contains("Corridors")))
            {
                _instructions.Add(" - Corridors: Highly branching tunnels generated using Prim's Algorithm.");
            }

            return this;
        }

        // Metoda do dodawania kilku mniejszych pokoi w losowych miejscach
        public IMapBuilder AddRooms(int n = 5)
        {
            int roomWidth = 8;
            int roomHeight = 4;

            for (int i = 0; i < 5; i++)
            {
                int rx = _rnd.Next(1, Constants.MazeWidth - roomWidth);
                int ry = _rnd.Next(1, Constants.MazeHeight - roomHeight);
                for (int y = ry; y < ry + roomHeight; y++)
                    for (int x = rx; x < rx + roomWidth; x++)
                        _cells![y, x].IsWall = false;
            }
            EnsureStructureHeader();
            if (!_instructions.Any(s => s.Contains("Rooms")))
            {
                _instructions.Add($" - Rooms: {n} spacious chambers. Explore them to find loot!");
            }
            return this;
        }

        // Metoda do losowego rozmieszczania przedmiotów na mapie
        public IMapBuilder AddRandomItems(int count)
        {
            CheckInitialized();
            PlaceRandomly(() => _itemFactory.GetRandomUnusable(), count);
            if (!_instructions.Any(s => s.Contains("Unusable items")))
            {
                _instructions.Add($" - Unusable items: There are {count} items placed on the map.");
            }

            return this;
        }

        // Metoda do losowego rozmieszczania broni na mapie
        public IMapBuilder AddRandomWeapons(int count)
        {
            CheckInitialized();
            PlaceRandomly(() => _itemFactory.GetRandomWeapon(), count);
            if (!_instructions.Any(s => s.Contains("Weapons")))
            {
                _instructions.Add($" - Weapons: There are {count} weapons placed on the map.");
            }

            return this;
        }

        public IMapBuilder SetEnemyNames(List<string> names)
        {
            _enemyNames = names;
            return this;
        }

        public IMapBuilder SetItemPool(List<Func<Item>> pool)
        {
            _itemPool = pool;
            return this;
        }

        public IMapBuilder AddRandomLoot(int count)
        {
            CheckInitialized();
            if (_itemPool.Count == 0) return this;

            int placed = 0;
            while (placed < count)
            {
                int rx = _rnd.Next(Constants.MazeWidth);
                int ry = _rnd.Next(Constants.MazeHeight);
                if (!_cells![ry, rx].IsWall)
                {
                    _cells[ry, rx].Items.Add(_itemPool[_rnd.Next(_itemPool.Count)]());
                    placed++;
                }
            }
            return this;
        }

        public IMapBuilder AddSpecificItem(Item item)
        {
            CheckInitialized();
            while (true)
            {
                int rx = _rnd.Next(Constants.MazeWidth);
                int ry = _rnd.Next(Constants.MazeHeight);
                if (!_cells![ry, rx].IsWall)
                {
                    _cells[ry, rx].Items.Add(item);
                    break;
                }
            }
            EnsureStructureHeader();
            _instructions.Add($" - Artifact: Hidden somewhere in the dungeon is the {item.Name}!");
            return this;
        }

        public IMapBuilder AddInstruction(string message)
        {
            _instructions.Insert(0, message);
            _instructions.Insert(1, "");
            return this;
        }

        public IMapBuilder AddEnemies(int count)
        {
            CheckInitialized();
            int placed = 0;

            if (!_speciesPublishers.ContainsKey("Goblin"))
                _speciesPublishers["Goblin"] = new SpeciesPublisher("Goblin");
            if (!_speciesPublishers.ContainsKey("Skeleton"))
                _speciesPublishers["Skeleton"] = new SpeciesPublisher("Skeleton");
            if (!_speciesPublishers.ContainsKey("Troll"))
                _speciesPublishers["Troll"] = new SpeciesPublisher("Troll");
            
            while (placed < count)
            {
                int rx = _rnd.Next(Constants.MazeWidth);
                int ry = _rnd.Next(Constants.MazeHeight);

                if (!_cells![ry, rx].IsWall && _cells[ry, rx].Enemy == null)
                {
                    int hp = _rnd.Next(20, 51);
                    int attack = _rnd.Next(5, 16);
                    int armor = _rnd.Next(0, 6);
                    EnemyStats stats = new EnemyStats(hp, attack, armor);

                    Enemy newEnemy;
                    SpeciesPublisher speciesPub;

                    int roll = _rnd.Next(3);
                    if (roll == 0)
                    {
                        newEnemy = new Goblin(stats);
                        speciesPub = _speciesPublishers["Goblin"];
                    }
                    else if (roll == 1)
                    {
                        newEnemy = new Skeleton(stats);
                        speciesPub = _speciesPublishers["Skeleton"];
                    }
                    else
                    {
                        newEnemy = new Troll(stats);
                        speciesPub = _speciesPublishers["Troll"];
                    }

                    newEnemy.InitializeSubscriptions(speciesPub, _soundPublisher, rx, ry);
                    
                    _cells[ry, rx].Enemy = newEnemy;
                    placed++;
                }
            }

            EnsureStructureHeader();
            if (!_instructions.Any(s => s.Contains("Enemies")))
            {
                _instructions.Add($" - Enemies: Watch out! There are {count} dangerous monsters lurking in the dark.");
            }

            return this;
        }

        // Metoda do losowego rozmieszczania przedmiotów na mapie
        private void PlaceRandomly(Func<Item> itemGenerator, int count)
        {
            int placed = 0;
            while (placed < count)
            {
                int rx = _rnd.Next(Constants.MazeWidth);
                int ry = _rnd.Next(Constants.MazeHeight);
                if (!_cells![ry, rx].IsWall)
                {
                    _cells[ry, rx].Items.Add(itemGenerator());
                    placed++;
                }
            }
        }
        
        // umieszcza losowe kamienie i holdery na mapie
        public IMapBuilder AddRandomSlotItems(int count)
        {
            CheckInitialized();
            PlaceRandomly(() => _itemFactory.GetRandomSlotItem(), count);
            if (!_instructions.Any(s => s.Contains("Slot items")))
            {
                _instructions.Add($" - Slot items: There are {count} passive items and holders scattered around.");
            }
            return this;
        }

        // Metoda do zwracania gotowej mapy wraz z instrukcjami dla gracza
        public Map GetResult()
        {
            CheckInitialized();

            _instructions.Add("");
            _instructions.Add("Press [ENTER] to start the game...");

            Map finalMap = new Map(_cells!, new List<string>(_instructions), _soundPublisher);

            _cells = null;

            return finalMap;
        }
    }
}
