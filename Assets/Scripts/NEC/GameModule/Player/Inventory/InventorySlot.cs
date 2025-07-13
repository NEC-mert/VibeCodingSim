namespace NEC.GameModule.Player.Inventory
{
    /// <summary>
    /// Represents a single slot in an inventory. Not a MonoBehaviour.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int quantity;

        // Constructor to create an empty slot
        public InventorySlot()
        {
            itemData = null;
            quantity = 0;
        }

        // You can add helper methods here later, e.g., AddQuantity, ClearSlot, etc.
    }
}