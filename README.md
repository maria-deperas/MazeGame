# MazeGame

Konsolowa gra w labirynt napisana w **C# / .NET 8**, z trybem lokalnym oraz sieciowym trybem wieloosobowym (klient–serwer przez TCP).

Projekt powstał na przedmiot **Projektowanie Obiektowe**. Był rozwijany iteracyjnie w 7 etapach. Każdy etap dokładał nowe wymagania, więc kod musiał pozostać otwarty na rozszerzenia i łatwy do zmiany. Głównym celem było świadome stosowanie zasad **SOLID** i **wzorców projektowych**.

## Funkcje

- **Generowanie map**: proceduralne lochy w kilku motywach (Garden, Library, Vault).
- **System przedmiotów**: broń jedno- i dwuręczna, przedmioty pasywne zwiększające atrybuty, waluta i przedmioty fabularne. Przedmioty można podnosić, upuszczać i zakładać do lewej lub prawej ręki.
- **Modyfikatory broni**: np. *Strong* i *Unlucky*, które można dowolnie łączyć.
- **Walka**: trzy rodzaje ataku (zwykły, z ukrycia i magiczny), których skuteczność zależy od użytej broni.
- **Przeciwnicy z AI**: Goblin, Szkielet i Troll. Każdy ma własną strategię zachowania (podążanie za graczem, ucieczka, reakcja na dźwięk albo ignorowanie gracza).
- **Multiplayer**: autorytatywny serwer zarządza stanem gry, a klienci wysyłają akcje i odbierają stan w formacie JSON.
- **Interfejs konsolowy**: kolorowe panele (atrybuty, ekwipunek, plecak, walka, log zdarzeń).
- **Logowanie**: zapis zdarzeń do pliku i do pamięci, z podglądem w grze.

## Architektura

Projekt jest zbudowany w oparciu o **MVC**, a warstwa sieciowa jest wydzielona osobno:

```
Controller/   obsługa wejścia, komendy, punkt wejścia (Program.cs)
Model/        logika gry: postacie, przedmioty, walka, świat, zdarzenia, DTO
View/         renderowanie: widok lokalny, widok klienta, widok serwera
Network/      serwer TCP, klient, sesje, protokół JSON
Core/         stałe i konfiguracja klawiszy
```

Ten sam model gry obsługuje tryb lokalny i sieciowy. Zmienia się tylko widok i źródło wejścia (`IGameView`, `IInputHandler`).

## Zastosowane wzorce projektowe

| Wzorzec | Zastosowanie |
|---|---|
| **Builder + Director** | Budowanie mapy krok po kroku (`StandardMapBuilder`, `MapDirector`, `ThemeDungeonDirector`) |
| **Abstract Factory** | Rodziny przedmiotów i przeciwników dla danego motywu (`GardenThemeFactory`, `LibraryThemeFactory`, `VaultThemeFactory`) |
| **Strategy** | Zachowania przeciwników (`IEnemyBehavior`) i strategie generowania map (`IMapStrategy`) |
| **Decorator** | Modyfikatory broni (`StrongDecorator`, `UnluckyDecorator`) |
| **Visitor** | Rodzaje ataku, które różnie działają na różne typy broni (`ICombatVisitor`) |
| **Command** | Każda akcja gracza i operacja w menu to osobna komenda (`ICommand`, `IMenuCommand`) |
| **Chain of Responsibility** | Łańcuch obsługi klawiszy: menu, ruch, akcje (`IInputHandler.SetNext`) |
| **Observer** | Zdarzenia dźwięku i gatunków, na które reagują przeciwnicy (`SoundPublisher`, `SpeciesPublisher`) |
| **Singleton** | Globalny dziennik zdarzeń (`GameLog`) |
| **Composite** | Panele interfejsu składane w kolumny (`VerticalStackPanel`, `IRenderable`) |
| **DTO + Mapper** | Serializacja stanu gry do przesłania przez sieć (`GameStateMapper`, `*Dto`) |

## Uruchomienie

Wymagany jest [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

Po starcie program pyta o tryb: **(S)erwer**, **(K)lient** albo **(L)okalnie**. Tryb można też podać w argumentach:

```bash
# serwer (domyślnie port 5555)
dotnet run -- --server 5555

# klient
dotnet run -- --client 127.0.0.1:5555
```

Ustawienia trybu lokalnego (imię gracza, ścieżka logów) znajdują się w pliku `config.json`.

## Sterowanie

| Klawisz | Akcja |
|---|---|
| `W` `A` `S` `D` | Ruch |
| `E` | Przedmioty na ziemi |
| `B` | Plecak |
| `J` | Dziennik zdarzeń |
| `↑` `↓` | Wybór w menu |
| `Enter` | Podnieś przedmiot |
| `L` / `R` | Załóż do lewej / prawej ręki |
| `1` / `2` | Zdejmij z lewej / prawej ręki |
| `D` (w menu) | Upuść przedmiot |
| `1` `2` `3` (w walce) | Atak zwykły / z ukrycia / magiczny |
| `Esc` | Zamknij menu |

## Technologie

- C# / .NET 8
- `System.Net.Sockets` (TCP), `System.Text.Json`
- [Pastel](https://github.com/silkfire/Pastel) do kolorowania tekstu w konsoli
