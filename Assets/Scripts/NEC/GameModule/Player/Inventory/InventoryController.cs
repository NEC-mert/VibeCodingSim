using System.Collections.Generic;
using NEC.SaveModule;
using UnityEngine;

namespace NEC.GameModule.Player.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        // --- NO CHANGE TO EXISTING FIELDS ---
        [Header("Inventory Settings")]
        [Tooltip("The number of slots in the hotbar.")]
        public int hotbarSize = 6;
        [Tooltip("The number of columns in the main bag.")]
        public int bagWidth = 5;
        [Tooltip("The number of rows in the main bag.")]
        public int bagHeight = 4;

        [Header("Item Holder")]
        [Tooltip("The transform parented to the player's camera that will hold the item model.")]
        public Transform itemHolder;
    
        // --- NEW FIELD FOR ITEM DATABASE ---
        [Header("Item Database")]
        [Tooltip("A list of ALL possible items in the game. This is used for saving and loading.")]
        public List<ItemData> allPossibleItems;

        // --- PRIVATE VARIABLES (with additions) ---
        private InventorySlot[] hotbarSlots;
        private InventorySlot[,] bagSlots;

        public int activeSlotIndex = 0;
        private GameObject currentHeldItem;
    
        // Quick-lookup dictionary for finding items by name
        private Dictionary<string, ItemData> itemDatabase;

        // --- Helper class for Serialization ---
        // This class is a "snapshot" of the inventory state using simple data types.
        [System.Serializable]
        private class SerializableInventoryData
        {
            public string[] hotbarItemNames;
            public int[] hotbarItemQuantities;
            public string[] bagItemNames;
            public int[] bagItemQuantities;
            public int activeSlotIndex;

            public SerializableInventoryData(int hotbarSize, int bagSize)
            {
                hotbarItemNames = new string[hotbarSize];
                hotbarItemQuantities = new int[hotbarSize];
                bagItemNames = new string[bagSize];
                bagItemQuantities = new int[bagSize];
            }
        }


        // --- UNITY METHODS (Modified) ---

        // Changed Start to Awake to ensure the database is ready before other scripts might need it.
        private void Awake()
        {
            // --- SETUP ITEM DATABASE ---
            itemDatabase = new Dictionary<string, ItemData>();
            foreach (var item in allPossibleItems)
            {
                if (item != null && !itemDatabase.ContainsKey(item.name))
                {
                    itemDatabase.Add(item.name, item);
                }
            }

            // --- INITIALIZE INVENTORIES ---
            InitializeInventories();
        
            // --- LOAD SAVED DATA ---
            LoadInventory();
        }
    
        // Added for demonstration of when to save
        private void OnApplicationQuit()
        {
            SaveInventory();
        }

        private void Update()
        {
            // --- Input for Saving/Loading (for testing) ---
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveInventory();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadInventory();
            }

            // --- NO CHANGE TO EXISTING INPUT LOGIC ---
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                if (scroll > 0) activeSlotIndex--; else activeSlotIndex++;
                if (activeSlotIndex >= hotbarSize) activeSlotIndex = 0;
                if (activeSlotIndex < 0) activeSlotIndex = hotbarSize - 1;
                SelectSlot(activeSlotIndex);
            }
            for (int i = 0; i < hotbarSize; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectSlot(i);
                }
            }
        }
    
        // --- NEW AND MODIFIED METHODS ---

        private void InitializeInventories()
        {
            hotbarSlots = new InventorySlot[hotbarSize];
            for (int i = 0; i < hotbarSize; i++)
            {
                hotbarSlots[i] = new InventorySlot();
            }

            bagSlots = new InventorySlot[bagWidth, bagHeight];
            for (int x = 0; x < bagWidth; x++)
            {
                for (int y = 0; y < bagHeight; y++)
                {
                    bagSlots[x, y] = new InventorySlot();
                }
            }
        }

        private void SaveInventory()
        {
            // 1. Create a data object to hold the inventory state.
            SerializableInventoryData saveData = new SerializableInventoryData(hotbarSize, bagWidth * bagHeight);

            // 2. Populate the data object from our live inventory.
            for (int i = 0; i < hotbarSize; i++)
            {
                if (hotbarSlots[i].itemData != null)
                {
                    saveData.hotbarItemNames[i] = hotbarSlots[i].itemData.name;
                    saveData.hotbarItemQuantities[i] = hotbarSlots[i].quantity;
                }
            }

            for (int y = 0; y < bagHeight; y++)
            {
                for (int x = 0; x < bagWidth; x++)
                {
                    int index = y * bagWidth + x;
                    if (bagSlots[x, y].itemData != null)
                    {
                        saveData.bagItemNames[index] = bagSlots[x, y].itemData.name;
                        saveData.bagItemQuantities[index] = bagSlots[x, y].quantity;
                    }
                }
            }
        
            saveData.activeSlotIndex = this.activeSlotIndex;

            // 3. Convert the data object to a JSON string.
            string json = JsonUtility.ToJson(saveData, true);

            // 4. Save the string using your Memory system.
            Memory.SaveString(StringSave.InventoryState, json);

            Debug.Log("Inventory Saved!");
        }

        private void LoadInventory()
        {
            // 1. Load the JSON string from your Memory system.
            string json = Memory.LoadString(StringSave.InventoryState);

            if (string.IsNullOrEmpty(json))
            {
                // No saved data found, select default slot and exit.
                SelectSlot(0);
                return;
            }

            // 2. Create a data object and overwrite it with the loaded JSON.
            SerializableInventoryData loadData = new SerializableInventoryData(hotbarSize, bagWidth * bagHeight);
            JsonUtility.FromJsonOverwrite(json, loadData);
        
            // 3. Repopulate the live inventory from the loaded data.
            for (int i = 0; i < hotbarSize; i++)
            {
                if (!string.IsNullOrEmpty(loadData.hotbarItemNames[i]))
                {
                    // Find the ItemData in our database by its saved name
                    if(itemDatabase.TryGetValue(loadData.hotbarItemNames[i], out ItemData item))
                    {
                        hotbarSlots[i].itemData = item;
                        hotbarSlots[i].quantity = loadData.hotbarItemQuantities[i];
                    }
                }
            }

            for (int y = 0; y < bagHeight; y++)
            {
                for (int x = 0; x < bagWidth; x++)
                {
                    int index = y * bagWidth + x;
                    if (!string.IsNullOrEmpty(loadData.bagItemNames[index]))
                    {
                        if (itemDatabase.TryGetValue(loadData.bagItemNames[index], out ItemData item))
                        {
                            bagSlots[x, y].itemData = item;
                            bagSlots[x, y].quantity = loadData.bagItemQuantities[index];
                        }
                    }
                }
            }
        
            // 4. Restore the active slot and update the visuals.
            SelectSlot(loadData.activeSlotIndex);

            Debug.Log("Inventory Loaded!");
        }

        // --- NO CHANGE TO METHODS BELOW ---
        private void SelectSlot(int newIndex)
        {
            if (newIndex < 0 || newIndex >= hotbarSize) return;
            activeSlotIndex = newIndex;
        
            ItemData selectedItem = hotbarSlots[activeSlotIndex].itemData;
            if (selectedItem != null) Debug.Log($"Selected slot {activeSlotIndex + 1}: {selectedItem.itemName}");
            else Debug.Log($"Selected slot {activeSlotIndex + 1}: Empty");

            UpdateHeldItem();
        }

        private void UpdateHeldItem()
        {
            if (itemHolder == null) return;
            if (currentHeldItem != null) Destroy(currentHeldItem);
        
            ItemData itemToHold = hotbarSlots[activeSlotIndex].itemData;
            if (itemToHold != null && itemToHold.inHandPrefab != null)
            {
                currentHeldItem = Instantiate(itemToHold.inHandPrefab, itemHolder);
                currentHeldItem.transform.localPosition = Vector3.zero;
                currentHeldItem.transform.localRotation = Quaternion.identity;
            }
        }

        public void MoveItemFromBagToHotbar(int bagX, int bagY, int hotbarIndex)
        {
            InventorySlot bagSlot = bagSlots[bagX, bagY];
            InventorySlot hotbarSlot = hotbarSlots[hotbarIndex];
        
            ItemData tempData = hotbarSlot.itemData;
            int tempQty = hotbarSlot.quantity;
        
            hotbarSlot.itemData = bagSlot.itemData;
            hotbarSlot.quantity = bagSlot.quantity;
        
            bagSlot.itemData = tempData;
            bagSlot.quantity = tempQty;
        
            if (hotbarIndex == activeSlotIndex)
            {
                UpdateHeldItem();
            }
        }

        public InventorySlot GetBagSlot(int x, int y)
        {
            if (x < 0 || x >= bagWidth || y < 0 || y >= bagHeight)
                return null;
            return bagSlots[x, y];
        }

        public InventorySlot GetHotbarSlot(int index)
        {
            if (index < 0 || index >= hotbarSize)
                return null;
            return hotbarSlots[index];
        }
    }
}