using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MazeGame
{
    // None - labirynt
    // Ground - przegądanie przedmiotów na danym polu
    // Backpack - przeglądanie przedmiotów w plecaku
    public enum MenuMode { None, Ground, Backpack }

    public class MenuManager
    {
        public MenuMode CurrentMenu { get; private set; } = MenuMode.None; // gdzie aktualnie jesteśmy
        public int SelectionIndex { get; private set; } = -1; // który przedmiot na liście przeglądamy
        public bool IsInMenuMode => CurrentMenu != MenuMode.None; // czy jesteśmy w jakimś menu
        
        // wywoływana, gd użytkownik wciśnie 'E' - otwieranie menu pola
        public void ToggleGroundMenu(Player player, Map map, Action<string> showMessage)
        {
            Cell currentCell = map.GetCell(player.x, player.y);
            if (!currentCell.HasItem) return; // jeśli nie ma przedmiotu nic się nie otwiera

            // jeśli jest jeden przedmiot bierzemy go od razu
            if (currentCell.Items.Count == 1)
            {
                if (currentCell.Items[0].PickUp(player))
                {
                    currentCell.Items.RemoveAt(0);
                }
                else showMessage("Backpack is full!");
            }
            // jeśli > 1 przedmiotów przechodzimy do menu z wyborem
            else
            {
                CurrentMenu = MenuMode.Ground;
                SelectionIndex = 0;
            }
        }

        // wywoływana, gd użytkownik wciśnie 'B' - otwieranie menu plecaka
        public void ToggleBackpackMenu(Player player)
        {
            // ustawiamy kursor na pierwszy przedmiot
            if (player.Backpack.Count > 0)
            {
                CurrentMenu = MenuMode.Backpack;
                SelectionIndex = 0;
            }
        }

        // jeśli wciśnięty został klawisz przekierowuje do funkcji dla odpowiedniego
        public void HandleMenuInput(Player player, Map map, ConsoleKey key, Action<string> showMessage)
        {
            if (CurrentMenu == MenuMode.Ground) 
                HandleGroundInput(player, map, key, showMessage);
            else if (CurrentMenu == MenuMode.Backpack) 
                HandleBackpackInput(player, map, key, showMessage);
        }

        private void HandleGroundInput(Player player, Map map, ConsoleKey key, Action<string> showMessage)
        {
            Cell currentCell = map.GetCell(player.x, player.y);
            // jeśli nie ma przedmiotu na polu -> zamykamy menu
            if (!currentCell.HasItem) 
            { 
                CloseMenu(); 
                return; 
            }

            switch (key)
            {
                // przchodzenie po liście
                case ConsoleKey.UpArrow:
                    SelectionIndex = (SelectionIndex - 1 + currentCell.Items.Count) % currentCell.Items.Count;
                    break;
                case ConsoleKey.DownArrow:
                    SelectionIndex = (SelectionIndex + 1) % currentCell.Items.Count;
                    break;
                // podnoszenie przedmiotu
                case ConsoleKey.Enter:
                    Item selected = currentCell.Items[SelectionIndex];
                    if (selected.PickUp(player))
                    {
                        currentCell.Items.RemoveAt(SelectionIndex);
                        CloseMenu();
                    }
                    else showMessage("Backpack is full, you can't pick up " + selected.Name);
                    break;
                // wciśnięcie escape zamyka menu
                case ConsoleKey.Escape:
                    CloseMenu();
                    break;
            }
        }

        private void HandleBackpackInput(Player player, Map map, ConsoleKey key, Action<string> showMessage)
        {
            int cnt = player.Backpack.Count;
            if (cnt == 0) { 
                CloseMenu(); 
                return; 
            }

            if (SelectionIndex >= cnt)
            {
                SelectionIndex = cnt - 1;
            }

            Item selected = player.Backpack[SelectionIndex];
            
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    SelectionIndex = (SelectionIndex - 1 + cnt) % cnt;
                    break;
                case ConsoleKey.DownArrow:
                    SelectionIndex = (SelectionIndex + 1) % cnt;
                    break;
                case ConsoleKey.R:
                    if (!selected.AddToRight(player))
                    {
                        showMessage("Right hand can't be emptied!");
                    }
                    break;
                case ConsoleKey.L:
                    if (!selected.AddToLeft(player))
                    {
                        showMessage("Left hand can't be emptied!");
                    }
                    break;
                case ConsoleKey.D:
                    if (selected.Drop(player, map.GetCell(player.x, player.y)))
                    {
                        if (cnt == 0) CloseMenu();
                        else SelectionIndex = Math.Max(0, SelectionIndex - 1);
                    }
                    else showMessage("Too many items on this tile!");
                    break;
                case ConsoleKey.Escape:
                    CloseMenu();
                    break;
            }
        }

        private void CloseMenu()
        {
            CurrentMenu = MenuMode.None;
            SelectionIndex = -1;
        }
    }
}
