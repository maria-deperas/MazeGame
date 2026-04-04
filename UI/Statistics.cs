using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.Characters;
using MazeGame.Core;
using MazeGame.World;
/*
namespace MazeGame.UI
{
    public class Statistics
    {
        int w = Constants.StatColumnWidth;
        public List<string> GetStatLines(Player player, Cell currentCell, MenuMode mode, int selectedIndex)
        {
            List<string> lines = new List<string>();

            // Informacje o graczu
            lines.Add(MakeHeader("PLAYER"));
            lines.Add($"Position: [{player.X}, {player.Y}]".PadRight(w));

            // Informacje o przedmiotach w rękach gracza
            string rightItem = player.RightHand != null ? player.RightHand.Name : "Empty";
            string leftItem = player.LeftHand != null ? player.LeftHand.Name : "Empty";

            // Wyświetlanie przedmiotów w rękach gracza
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
                left = right;

            if (player.LeftHand != null && player.LeftHand.IsTwoHanded)
                right = left;

            lines.Add($"Left Hand: {left}".PadRight(w));
            lines.Add($"Right Hand: {right}".PadRight(w));

            lines.Add(MakeHeader("ON GROUND"));
            for (int i = 0; i < Constants.MaxItemsPerCell; i++)
            {
                if (i < currentCell.Items.Count)
                {
                    string prefix = (mode == MenuMode.Ground && i == selectedIndex) ? " > " : "   ";
                    lines.Add($"{prefix}{currentCell.Items[i].GetInfo()}".PadRight(w));
                }
                else
                {
                    string text = (i == 0 && !currentCell.HasItem) ? "   No items here." : "";
                    lines.Add(text.PadRight(w));
                }
            }

            // Plecak gracza
            lines.Add(MakeHeader("BACKPACK"));
            for (int i = 0; i < Constants.BackpackCapacity; i++)
            {
                if (i < player.Backpack.Count)
                {
                    string prefix = (mode == MenuMode.Backpack && i == selectedIndex) ? " > " : "   ";
                    lines.Add($"{prefix}{i + 1}. {player.Backpack[i].GetInfo()}".PadRight(w));
                }
                else
                {
                    lines.Add($"   {i + 1}. [ Empty ]".PadRight(w));
                }
            }

            // Atrybuty gracza
            lines.Add(MakeHeader("ATTRIBUTES"));

            lines.Add(string.Format(" Strength:   {0,-4} Agility: {1}", player.Stats.Strength, player.Stats.Agility).PadRight(w));
            lines.Add(string.Format(" Health:     {0,-4} Luck:    {1}", player.Stats.Health, player.Stats.Luck).PadRight(w));
            lines.Add(string.Format(" Aggression: {0,-4} Wisdom:  {1}", player.Stats.Aggression, player.Stats.Wisdom).PadRight(w));

            // Zebrane monety i złoto
            lines.Add(MakeHeader("COLLECTED"));
            lines.Add($" Coins: {player.Coins}".PadRight(w));
            lines.Add($" Gold:  {player.Gold}".PadRight(w));

            return lines;

        }

        string MakeHeader(string title)
        {
            string paddedTitle = $" {title} ";
            if (paddedTitle.Length >= w) return paddedTitle.Substring(0, w);

            int leftPad = (w - paddedTitle.Length) / 2;
            int rightPad = w - paddedTitle.Length - leftPad;

            return new string('─', leftPad) + paddedTitle + new string('─', rightPad);
        }
    }
}*/