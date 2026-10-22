# MazeGame

A console maze game written in **C# / .NET 8**, with a local single-player mode and a networked multiplayer mode (client–server over TCP).

Built for the **Object-Oriented Design** course at university. The project was developed iteratively over 7 stages. Each stage added new requirements, so the code had to stay open for extension and easy to change. The main goal was the deliberate use of **SOLID** principles and **design patterns**.

## Features

- **Map generation**: procedurally generated dungeons in several themes (Garden, Library, Vault).
- **Item system**: one- and two-handed weapons, passive items that boost attributes, currency and quest items. Items can be picked up, dropped and equipped in the left or right hand.
- **Weapon modifiers**: e.g. *Strong* and *Unlucky*, which can be freely stacked.
- **Combat**: three attack types (normal, stealth and magic), whose effectiveness depends on the weapon used.
- **Enemy AI**: Goblin, Skeleton and Troll. Each has its own behavior strategy (following the player, fleeing, reacting to sound or ignoring the player).
- **Multiplayer**: an authoritative server manages the game state, while clients send actions and receive state updates as JSON.
- **Console UI**: colored panels (attributes, equipment, backpack, combat, event log).
- **Logging**: events are written to a file and kept in memory, with an in-game log viewer.

## Architecture

The project follows **MVC**, with the networking layer kept separate:

```
Controller/   input handling, commands, entry point (Program.cs)
Model/        game logic: characters, items, combat, world, events, DTOs
View/         rendering: local view, client view, server view
Network/      TCP server, client, sessions, JSON protocol
Core/         constants and key bindings
```

The same game model drives both local and network modes. Only the view and the input source change (`IGameView`, `IInputHandler`).

## Design patterns

| Pattern | Usage |
|---|---|
| **Builder + Director** | Step-by-step map construction (`StandardMapBuilder`, `MapDirector`, `ThemeDungeonDirector`) |
| **Abstract Factory** | Families of items and enemies for a given theme (`GardenThemeFactory`, `LibraryThemeFactory`, `VaultThemeFactory`) |
| **Strategy** | Enemy behaviors (`IEnemyBehavior`) and map generation strategies (`IMapStrategy`) |
| **Decorator** | Weapon modifiers (`StrongDecorator`, `UnluckyDecorator`) |
| **Visitor** | Attack types that act differently on different weapon types (`ICombatVisitor`) |
| **Command** | Every player action and menu operation is a separate command (`ICommand`, `IMenuCommand`) |
| **Chain of Responsibility** | Key handling chain: menu, movement, actions (`IInputHandler.SetNext`) |
| **Observer** | Sound and species events that enemies react to (`SoundPublisher`, `SpeciesPublisher`) |
| **Singleton** | Global event log (`GameLog`) |
| **Composite** | UI panels stacked into columns (`VerticalStackPanel`, `IRenderable`) |
| **DTO + Mapper** | Serializing game state for network transfer (`GameStateMapper`, `*Dto`) |

## Running

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

On startup the program asks for a mode: **(S)erver**, **(K)lient** or **(L)ocal**. You can also pass the mode as an argument:

```bash
# server (default port 5555)
dotnet run -- --server 5555

# client
dotnet run -- --client 127.0.0.1:5555
```

Local mode settings (player name, log path) are in `config.json`.

## Controls

| Key | Action |
|---|---|
| `W` `A` `S` `D` | Move |
| `E` | Items on the ground |
| `B` | Backpack |
| `J` | Event log |
| `↑` `↓` | Menu selection |
| `Enter` | Pick up item |
| `L` / `R` | Equip in left / right hand |
| `1` / `2` | Unequip left / right hand |
| `D` (in menu) | Drop item |
| `1` `2` `3` (in combat) | Normal / stealth / magic attack |
| `Esc` | Close menu |

## Tech stack

- C# / .NET 8
- `System.Net.Sockets` (TCP), `System.Text.Json`
- [Pastel](https://github.com/silkfire/Pastel) for colored console output
