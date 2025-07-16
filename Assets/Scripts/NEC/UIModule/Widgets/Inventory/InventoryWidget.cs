using NEC.GameModule.Player.Inventory;
using UnityEngine;

namespace NEC.UIModule.Widgets.Inventory
{
    public class InventoryWidget : AWidget
    {
        [Header("Inventory References")]
        [SerializeField] private InventoryController inventoryController;
        
        [Header("UI Components")]
        [SerializeField] private Transform bagSlotsParent;
        [SerializeField] private GameObject inventorySlotPrefab;
        
        private InventorySlotUI[,] _bagSlotUIs;
        
        protected override void OnShow()
        {
            base.OnShow();
            CreateBagSlots();
            RefreshInventoryDisplay();
        }
        
        private void CreateBagSlots()
        {
            if (bagSlotsParent == null || inventorySlotPrefab == null || inventoryController == null)
                return;
                
            foreach (Transform child in bagSlotsParent)
            {
                Destroy(child.gameObject);
            }
            
            int bagWidth = inventoryController.bagWidth;
            int bagHeight = inventoryController.bagHeight;
            
            _bagSlotUIs = new InventorySlotUI[bagWidth, bagHeight];
            
            for (int y = 0; y < bagHeight; y++)
            {
                for (int x = 0; x < bagWidth; x++)
                {
                    GameObject slotObj = Instantiate(inventorySlotPrefab, bagSlotsParent);
                    InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
                    
                    if (slotUI != null)
                    {
                        _bagSlotUIs[x, y] = slotUI;
                        slotUI.OnSlotClicked += OnBagSlotClicked;
                        slotUI.OnSlotHovered += OnBagSlotHovered;
                        
                        var slotData = inventoryController.GetBagSlot(x, y);
                        slotUI.SetSlotData(slotData, x, y);
                    }
                }
            }
        }
        
        private void OnBagSlotClicked(int x, int y)
        {
            Debug.Log($"Bag slot clicked: ({x}, {y})");
            var slot = inventoryController.GetBagSlot(x, y);
            if (slot != null && slot.itemData != null)
            {
                Debug.Log($"Item: {slot.itemData.itemName}, Quantity: {slot.quantity}");
            }
        }
        
        private void OnBagSlotHovered(int x, int y)
        {
            var slot = inventoryController.GetBagSlot(x, y);
            if (slot != null && slot.itemData != null)
            {
                Debug.Log($"Hovering over: {slot.itemData.itemName} x{slot.quantity}");
            }
        }
        
        public void RefreshInventoryDisplay()
        {
            if (inventoryController == null || _bagSlotUIs == null)
                return;
                
            int bagWidth = inventoryController.bagWidth;
            int bagHeight = inventoryController.bagHeight;
            
            for (int y = 0; y < bagHeight; y++)
            {
                for (int x = 0; x < bagWidth; x++)
                {
                    if (_bagSlotUIs[x, y] != null)
                    {
                        var slot = inventoryController.GetBagSlot(x, y);
                        _bagSlotUIs[x, y].SetSlotData(slot, x, y);
                    }
                }
            }
        }
        
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            
            if (_bagSlotUIs != null)
            {
                foreach (var slotUI in _bagSlotUIs)
                {
                    if (slotUI != null)
                    {
                        slotUI.OnSlotClicked -= OnBagSlotClicked;
                        slotUI.OnSlotHovered -= OnBagSlotHovered;
                    }
                }
            }
        }
    }
}