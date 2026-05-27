using MazeGame.Core;
using MazeGame.Network.Dto;

namespace MazeGame.UI.Panels
{
    // ─── Panel atrybutów gracza ───────────────────────────────────────────────
    public class ClientAttributesPanel : FramedPanel
    {
        private readonly Func<PlayerDto?> _getPlayer;

        public ClientAttributesPanel(Func<PlayerDto?> getPlayer)
            : base(Constants.LeftPanelWidth, 4, "ATTRIBUTES")
        {
            _getPlayer = getPlayer;
        }

        protected override List<string> GetContentLines()
        {
            var p = _getPlayer();
            if (p?.Stats == null)
                return new List<string> { " (oczekiwanie...)", "", "", "" };

            return new List<string>
            {
                $" Pos: [{p.X}, {p.Y}]".PadRight(Width - 2),
                $" Str: {p.Stats.Strength,-4} Agi: {p.Stats.Agility}",
                $" HP:  {p.Stats.Health,-4} Lck: {p.Stats.Luck}",
                $" Agr: {p.Stats.Aggression,-4} Wis: {p.Stats.Wisdom}"
            };
        }
    }

    // ─── Panel portfela ───────────────────────────────────────────────────────
    public class ClientWalletPanel : FramedPanel
    {
        private readonly Func<PlayerDto?> _getPlayer;

        public ClientWalletPanel(Func<PlayerDto?> getPlayer)
            : base(Constants.LeftPanelWidth, 2, "WALLET")
        {
            _getPlayer = getPlayer;
        }

        protected override List<string> GetContentLines()
        {
            var p = _getPlayer();
            return new List<string>
            {
                $" Coins: {p?.Coins ?? 0}",
                $" Gold:  {p?.Gold ?? 0}"
            };
        }
    }

    // ─── Panel sterowania (kontekstowy) ──────────────────────────────────────
    public class ClientControlsPanel : FramedPanel
    {
        private readonly Func<List<string>> _getInstructions;

        public ClientControlsPanel(Func<List<string>> getInstructions)
            : base(Constants.LeftPanelWidth, 6, "CONTROLS")
        {
            _getInstructions = getInstructions;
        }

        protected override List<string> GetContentLines()
        {
            var lines = _getInstructions();
            while (lines.Count < 6) lines.Add("");
            return lines.Take(6).ToList();
        }
    }

    // ─── Panel ekwipunku ──────────────────────────────────────────────────────
    public class ClientEquippedPanel : FramedPanel
    {
        private readonly Func<PlayerDto?> _getPlayer;

        public ClientEquippedPanel(Func<PlayerDto?> getPlayer)
            : base(Constants.RightPanelWidth, 2, "EQUIPPED")
        {
            _getPlayer = getPlayer;
        }

        protected override List<string> GetContentLines()
        {
            var p = _getPlayer();
            string l = p?.LeftHand?.Info  ?? "Empty";
            string r = p?.RightHand?.Info ?? "Empty";

            if (p?.RightHand?.IsTwoHanded == true) l = r;
            if (p?.LeftHand?.IsTwoHanded  == true) r = l;

            return new List<string> { $" L: {l}", $" R: {r}" };
        }
    }

    // ─── Przedmioty na ziemi ──────────────────────────────────────────────────
    public class ClientGroundPanel : FramedPanel
    {
        private readonly Func<List<ItemDto>?> _getItems;
        private readonly Func<int> _getSelection;

        public ClientGroundPanel(Func<List<ItemDto>?> getItems, Func<int> getSelection)
            : base(Constants.RightPanelWidth, Constants.MaxItemsPerCell, "ON GROUND")
        {
            _getItems    = getItems;
            _getSelection = getSelection;
        }

        protected override List<string> GetContentLines()
        {
            var items = _getItems();
            var lines = new List<string>();
            int sel   = _getSelection();

            for (int i = 0; i < Constants.MaxItemsPerCell; i++)
            {
                if (items != null && i < items.Count)
                {
                    string pfx = (i == sel) ? "> " : "  ";
                    lines.Add($"{pfx}{items[i].Name ?? "?"}");
                }
                else if (i == 0) lines.Add("  No items here.");
                else             lines.Add("");
            }
            return lines;
        }
    }

    // ─── Plecak ───────────────────────────────────────────────────────────────
    public class ClientBackpackPanel : FramedPanel
    {
        private readonly Func<PlayerDto?> _getPlayer;
        private readonly Func<int>        _getSelection;

        public ClientBackpackPanel(Func<PlayerDto?> getPlayer, Func<int> getSelection)
            : base(Constants.RightPanelWidth, Constants.BackpackCapacity, "BACKPACK")
        {
            _getPlayer    = getPlayer;
            _getSelection = getSelection;
        }

        protected override List<string> GetContentLines()
        {
            var p   = _getPlayer();
            int sel = _getSelection();
            var lines = new List<string>();

            for (int i = 0; i < Constants.BackpackCapacity; i++)
            {
                if (p != null && i < p.Backpack.Count)
                {
                    string pfx = (i == sel) ? "> " : "  ";
                    var item = p.Backpack[i];
                    string info = item.Info ?? item.Name ?? "?";
                    lines.Add($"{pfx}{i + 1}. {info}");
                }
                else lines.Add($"  {i + 1}. [ Empty ]");
            }
            return lines;
        }
    }

    // ─── Panel walki ──────────────────────────────────────────────────────────
    public class ClientCombatPanel : FramedPanel
    {
        private readonly Func<EnemyDto?> _getEnemy;

        public ClientCombatPanel(Func<EnemyDto?> getEnemy)
            : base(Constants.RightPanelWidth, 4, "COMBAT")
        {
            _getEnemy = getEnemy;
        }

        protected override List<string> GetContentLines()
        {
            var enemy = _getEnemy();
            if (enemy == null)
                return new List<string> { "  No enemy.", "", "", "" };

            return new List<string>
            {
                $"  Enemy: {enemy.Name}",
                $"  HP:    {enemy.Stats?.CurrentHealth} / {enemy.Stats?.MaxHealth}",
                $"  Atk:   {enemy.Stats?.Attack}   Armor: {enemy.Stats?.Armor}",
                $"  [1] Normal  [2] Stealth  [3] Magic"
            };
        }
    }

    // ─── Lista graczy ─────────────────────────────────────────────────────────
    public class ClientPlayersPanel : FramedPanel
    {
        private readonly Func<List<PlayerDto>?> _getPlayers;
        private readonly Func<int>              _getLocalId;

        public ClientPlayersPanel(Func<List<PlayerDto>?> getPlayers, Func<int> getLocalId)
            : base(Constants.RightPanelWidth, 2, "PLAYERS")
        {
            _getPlayers = getPlayers;
            _getLocalId = getLocalId;
        }

        protected override List<string> GetContentLines()
        {
            var players = _getPlayers();
            int localId = _getLocalId();
            var lines   = new List<string>();

            if (players == null || players.Count == 0)
            {
                lines.Add("  (brak graczy)");
                lines.Add("");
                return lines;
            }

            // Pokaż tylko lokalnego gracza + jednego innego (mieścimy 2 linie)
            var local = players.FirstOrDefault(p => p.PlayerId == localId);
            if (local != null)
            {
                string hp = local.Stats != null ? $"HP:{local.Stats.Health}" : "HP:?";
                lines.Add($" *{local.PlayerId} {local.Name,-10} {hp} (ty)");
            }
            var others = players.Where(p => p.PlayerId != localId).Take(1);
            foreach (var p in others)
            {
                string hp = p.Stats != null ? $"HP:{p.Stats.Health}" : "HP:?";
                lines.Add($"  {p.PlayerId} {p.Name,-10} {hp}");
            }
            while (lines.Count < 2) lines.Add("");
            return lines;
        }
    }

    // ─── Dziennik zdarzeń (EventLog) ─────────────────────────────────────────
    public class ClientLogPanel : FramedPanel
    {
        private readonly Func<List<string>?> _getLogs;

        public ClientLogPanel(Func<List<string>?> getLogs)
            : base(Constants.RightPanelWidth, 4, "EVENT LOG")
        {
            _getLogs = getLogs;
        }

        protected override List<string> GetContentLines()
        {
            var logs  = _getLogs();
            var lines = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                if (logs != null && i < logs.Count)
                    lines.Add(" " + logs[i]);
                else
                    lines.Add("");
            }
            return lines;
        }
    }
}
