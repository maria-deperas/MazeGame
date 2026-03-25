using System;
using System.Collections.Generic;
using System.Drawing;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;
using Pastel;

namespace MazeGame.UI
{
    public class ActionPanel
    {
        public List<string> GetActionLines(Map map, Player player, MenuManager menu)
        {
            List<string> ui = new List<string>();
            int w = Constants.ActionColumnWidth;

            string MakeHeader(string title)
            {
                string paddedTitle = $" {title} ";
                if (paddedTitle.Length >= w) return paddedTitle.Substring(0, w);

                int leftPad = (w - paddedTitle.Length) / 2;
                int rightPad = w - paddedTitle.Length - leftPad;

                return new string('─', leftPad) + paddedTitle + new string('─', rightPad);
            }

            ui.Add(MakeHeader("AVAILABLE ACTIONS"));

            if (menu.CurrentMenu == MenuMode.None)
            {
                ui.Add("[W/A/S/D] Move");

                if (map.GetCell(player.X, player.Y).HasItem)
                {
                    ui.Add("[E] Pick up item");
                }

                if (player.Backpack.Count > 0)
                {
                    ui.Add("[B] Open backpack");
                }
            }
            else if (menu.CurrentMenu == MenuMode.Ground)
            {
                ui.Add("ON GROUND:");
                ui.Add("[↑ / ↓] Select item");
                ui.Add("[Enter] Pick up");
                ui.Add("[Esc]   Close");
            }
            else if (menu.CurrentMenu == MenuMode.Backpack)
            {
                ui.Add("YOUR BACKPACK:");
                ui.Add("[↑ / ↓] Select item");
                ui.Add("[R]     To right hand");
                ui.Add("[L]     To left hand");
                ui.Add("[D]     Drop item");
                ui.Add("[Esc]   Close");
            }

            return ui;
        }
    }
}
