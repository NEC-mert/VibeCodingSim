namespace NEC.GameModule.Player.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int quantity;

        public InventorySlot()
        {
            itemData = null;
            quantity = 0;
        }
    }
}