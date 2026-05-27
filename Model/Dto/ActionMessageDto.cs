namespace MazeGame.Network.Dto
{
    public enum ActionType
    {
        Move, Attack, PickUp,
        EquipLeft, EquipRight,
        UnequipLeft, UnequipRight,
        DropItem, CloseMenu
    }

    public enum AttackKind { Normal, Stealth, Magic }
    
    public class ActionMessageDto
    {
        public ActionType Type { get; set; }// typ akcji
        public int PlayerId { get; set; } // id gracza wykonującego akcję
        public int Dx { get; set; }
        public int Dy { get; set; }
        public AttackKind AttackType { get; set; }// typ ataku 
        public int SelectionIndex { get; set; } = -1; // indeks zaznaczonego przedmiotu (-1 -> nie dotyczy)
    }
}