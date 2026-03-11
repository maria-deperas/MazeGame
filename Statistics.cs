using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Statistics
    {
        public List<string> GetStatLines(Player player, Cell currentCell, MenuMode mode, int selectedIndex)
        {
            List<string> lines = new List<string>();
            int w = Constants.Width;

            lines.Add("────────────── PLAYER ──────────────".PadRight(w));
            lines.Add($"Position: [{player.x}, {player.y}]".PadRight(w));

            for (int i = 0; i < Constants.MaxItemsPerCell; i++)
            {
                if (i < currentCell.Items.Count)
                {
                    string prefix;
                    if (mode == MenuMode.Backpack && i == selectedIndex)
                    {
                        prefix = " > ";
                    }
                    else
                    {
                        prefix = "   ";
                    }
                    string itemInfo = $"{prefix}{currentCell.Items[i].GetInfo()}";

                    lines.Add(itemInfo.PadRight(w));
                }
                else
                {
                    string text = (i == 0 && !currentCell.HasItem) ? "   No items here." : "";
                    lines.Add(text.PadRight(w));
                }
            }

            string left;
            if (player.LeftHand != null)
                left = player.LeftHand.GetInfo();
            else
                left = "[ Empty ]";

            string right;
            if (player.RightHand != null)
                right = player.RightHand.GetInfo();
            else
                right = "[ Empty ]";

            if (player.RightHand != null && player.RightHand.IsTwoHanded) 
                left = "(Used by Right)";

            lines.Add($"Left Hand: {left}".PadRight(w));
            lines.Add($"Right Hand: {right}".PadRight(w));

            lines.Add("───────────── BACKPACK ─────────────".PadRight(w));
            for (int i = 0; i < Constants.BackpackCapacity; i++)
            {
                if (i < player.Backpack.Count)
                {
                    string prefix;
                    if(mode == MenuMode.Backpack && i == selectedIndex)
                    {
                        prefix = " > ";
                    }
                    else
                    {
                        prefix = "   ";
                    }
                    lines.Add($"{prefix}{i + 1}. {player.Backpack[i].GetInfo()}".PadRight(w));
                }
                else
                {
                    lines.Add($"   {i + 1}. [ Empty ]".PadRight(w));
                }
            }

            if (mode == MenuMode.Backpack)
                lines.Add("[R] Right | [L] Left | [D] Drop".PadRight(w));
            else
                lines.Add("(Press 'B' to open backpack)".PadRight(w));

            lines.Add("──────────── ATTRIBUTES ────────────".PadRight(w));
            lines.Add($"Strength:   {player.Stats.Strength,-5} Agility: {player.Stats.Agility}".PadRight(w));
            lines.Add($"Health:     {player.Stats.Health,-5} Luck:    {player.Stats.Luck}".PadRight(w));
            lines.Add($"Aggression: {player.Stats.Aggression,-5} Wisdom:  {player.Stats.Wisdom}".PadRight(w));

            lines.Add("───────────── COLLECTED ────────────".PadRight(w));
            lines.Add($"Coins: {player.Coins}".PadRight(w));
            lines.Add($"Gold:  {player.Gold}".PadRight(w));

            return lines;
        }
    }
}