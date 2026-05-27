using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.Items;
using MazeGame.Items.Weapons;
using MazeGame.Network.Dto;
using MazeGame.World;

namespace MazeGame.Network
{
    // tłumaczeniu obiektów modelu gry na dto i z powrotem
    public static class GameStateMapper
    {
        // mapa
        public static List<List<CellDto>> MapGrid(Map map)
        {
            var grid = new List<List<CellDto>>(Constants.MazeHeight);
            for (int y = 0; y < Constants.MazeHeight; y++)
            {
                var row = new List<CellDto>(Constants.MazeWidth);
                for (int x = 0; x < Constants.MazeWidth; x++)
                    row.Add(ToDto(map.GetCell(x, y)));
                grid.Add(row);
            }
            return grid;
        }

        // komórka
        public static CellDto ToDto(Cell cell)
        {
            return new CellDto
            {
                IsWall = cell.IsWall,
                Items = cell.Items.Select(ToDto).ToList(),
                Enemy = cell.HasEnemy ? ToDto(cell.Enemy!) : null
            };
        }

        // wróg
        public static EnemyDto ToDto(Enemy enemy)
        {
            return new EnemyDto
            {
                Name = enemy.Name,
                Symbol = enemy.Symbol,
                // System.Drawing.Color nie jest serializowany przez Json — zamieniamy na hex
                ColorHex = $"#{enemy.Color.R:X2}{enemy.Color.G:X2}{enemy.Color.B:X2}",
                X = enemy.X,
                Y = enemy.Y,
                Stats = ToDto(enemy.Stats)
            };
        }

        // statystyki wroga
        public static EnemyStatsDto ToDto(EnemyStats stats)
        {
            return new EnemyStatsDto
            {
                MaxHealth = stats.MaxHealth,
                CurrentHealth = stats.CurrentHealth,
                Attack = stats.Attack,
                Armor = stats.Armor
            };
        }

        // item
        public static ItemDto ToDto(Item item)
        {
            int damage = item is Weapon w ? w.Damage : 0;

            return new ItemDto
            {
                Name = item.Name,
                Symbol = item.Symbol,
                Damage = damage,
                IsEquippable = item.IsEquippable,
                IsTwoHanded  = item.IsTwoHanded,
                Info = item.GetInfo()
            };
        }

        // gracz
        public static PlayerDto ToDto(Player player, int playerId)
        {
            return new PlayerDto
            {
                PlayerId  = playerId,
                Name = player.Name,
                X = player.X,
                Y = player.Y,
                Stats = ToDto(player.Stats),
                Backpack = player.Backpack.Select(ToDto).ToList(),
                LeftHand  = player.LeftHand  != null ? ToDto(player.LeftHand)  : null,
                RightHand = player.RightHand != null ? ToDto(player.RightHand) : null,
                Coins = player.Coins,
                Gold = player.Gold
            };
        }

        public static AttributesDto ToDto(Attributes attrs)
        {
            return new AttributesDto
            {
                Strength = attrs.Strength,
                Agility = attrs.Agility,
                Health = attrs.Health,
                Luck = attrs.Luck,
                Aggression = attrs.Aggression,
                Wisdom = attrs.Wisdom
            };
        }
        
    }
}
