using UnityEngine;

namespace NEC.GameModule.Player.Inventory
{
    /// <summary>
    /// A ScriptableObject that defines an item's properties.
    /// Create new items by right-clicking in the Project window -> Create -> Inventory/Item
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        [Tooltip("The name of the item as it will appear in the UI.")]
        public string itemName = "New Item";

        [Tooltip("The icon for the item that will be shown in UI slots.")]
        public Sprite icon;

        [Tooltip("The 3D model prefab that will be instantiated in the player's hand.")]
        public GameObject inHandPrefab;
    }
}