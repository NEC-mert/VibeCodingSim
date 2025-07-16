using NEC.GameModule.Player.Inventory;
using UnityEngine;

namespace NEC.UIModule.Widgets.Hotbar
{
    public class HotbarWidget : AWidget
    {
        [Header("Hotbar References")]
        [SerializeField] private HotbarController hotbarController;
        
        [Header("UI Components")]
        [SerializeField] private Transform hotbarSlotsParent;
        [SerializeField] private GameObject hotbarSlotPrefab;
        
        private HotbarSlotUI[] _hotbarSlotUIs;
        
        protected override void OnShow()
        {
            base.OnShow();
            CreateHotbarSlots();
            RefreshHotbarDisplay();
        }
        
        protected override void AddListeners()
        {
            base.AddListeners();
            if (hotbarController != null)
            {
                hotbarController.OnActiveSlotChanged += OnActiveSlotChanged;
                hotbarController.OnHotbarUpdated += RefreshHotbarDisplay;
            }
        }
        
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            if (hotbarController != null)
            {
                hotbarController.OnActiveSlotChanged -= OnActiveSlotChanged;
                hotbarController.OnHotbarUpdated -= RefreshHotbarDisplay;
            }
            
            if (_hotbarSlotUIs != null)
            {
                foreach (var slotUI in _hotbarSlotUIs)
                {
                    if (slotUI != null)
                    {
                        slotUI.OnSlotClicked -= OnHotbarSlotClicked;
                        slotUI.OnSlotHovered -= OnHotbarSlotHovered;
                    }
                }
            }
        }
        
        private void CreateHotbarSlots()
        {
            if (hotbarSlotsParent == null || hotbarSlotPrefab == null || hotbarController == null)
                return;
                
            foreach (Transform child in hotbarSlotsParent)
            {
                Destroy(child.gameObject);
            }
            
            int hotbarSize = hotbarController.HotbarSize;
            _hotbarSlotUIs = new HotbarSlotUI[hotbarSize];
            
            for (int i = 0; i < hotbarSize; i++)
            {
                GameObject slotObj = Instantiate(hotbarSlotPrefab, hotbarSlotsParent);
                HotbarSlotUI slotUI = slotObj.GetComponent<HotbarSlotUI>();
                
                if (slotUI != null)
                {
                    _hotbarSlotUIs[i] = slotUI;
                    slotUI.OnSlotClicked += OnHotbarSlotClicked;
                    slotUI.OnSlotHovered += OnHotbarSlotHovered;
                    
                    var slotData = hotbarController.GetHotbarSlot(i);
                    slotUI.SetSlotData(slotData, i);
                    slotUI.SetSelected(i == hotbarController.ActiveSlotIndex);
                }
            }
        }
        
        private void OnHotbarSlotClicked(int index)
        {
            Debug.Log($"Hotbar slot clicked: {index}");
            if (hotbarController != null)
            {
                hotbarController.SelectSlot(index);
            }
        }
        
        private void OnHotbarSlotHovered(int index)
        {
            var slot = hotbarController?.GetHotbarSlot(index);
            if (slot != null && slot.itemData != null)
            {
                Debug.Log($"Hovering over hotbar slot {index + 1}: {slot.itemData.itemName} x{slot.quantity}");
            }
        }
        
        private void OnActiveSlotChanged(int newActiveIndex)
        {
            if (_hotbarSlotUIs == null) return;
            
            for (int i = 0; i < _hotbarSlotUIs.Length; i++)
            {
                if (_hotbarSlotUIs[i] != null)
                {
                    _hotbarSlotUIs[i].SetSelected(i == newActiveIndex);
                }
            }
        }
        
        public void RefreshHotbarDisplay()
        {
            if (hotbarController == null || _hotbarSlotUIs == null)
                return;
                
            int hotbarSize = hotbarController.HotbarSize;
            
            for (int i = 0; i < hotbarSize && i < _hotbarSlotUIs.Length; i++)
            {
                if (_hotbarSlotUIs[i] != null)
                {
                    var slot = hotbarController.GetHotbarSlot(i);
                    _hotbarSlotUIs[i].SetSlotData(slot, i);
                    _hotbarSlotUIs[i].SetSelected(i == hotbarController.ActiveSlotIndex);
                }
            }
        }
    }
}