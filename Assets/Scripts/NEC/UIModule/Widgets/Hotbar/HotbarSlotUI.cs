using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NEC.UIModule.Common;
using NEC.GameModule.Player.Inventory;
using System;

namespace NEC.UIModule.Widgets.Hotbar
{
    public class HotbarSlotUI : UIBase, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI Components")]
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text quantityText;
        [SerializeField] private Text slotNumberText;
        
        [Header("Visual Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color hoverColor = Color.cyan;
        [SerializeField] private Color emptyColor = Color.clear;
        
        private InventorySlot _slotData;
        private int _slotIndex;
        private bool _isSelected;
        
        public event Action<int> OnSlotClicked;
        public event Action<int> OnSlotHovered;
        
        public void SetSlotData(InventorySlot slot, int index)
        {
            _slotData = slot;
            _slotIndex = index;
            UpdateDisplay();
            
            if (slotNumberText != null)
            {
                slotNumberText.text = (index + 1).ToString();
            }
        }
        
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            UpdateBackgroundColor();
        }
        
        public void UpdateDisplay()
        {
            if (_slotData == null)
            {
                SetEmpty();
                return;
            }
            
            if (_slotData.itemData != null)
            {
                itemIcon.sprite = _slotData.itemData.icon;
                itemIcon.color = Color.white;
                
                if (quantityText != null)
                {
                    quantityText.text = _slotData.quantity > 1 ? _slotData.quantity.ToString() : "";
                    quantityText.gameObject.SetActive(_slotData.quantity > 1);
                }
            }
            else
            {
                SetEmpty();
            }
            
            UpdateBackgroundColor();
        }
        
        private void SetEmpty()
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.color = emptyColor;
            }
            
            if (quantityText != null)
            {
                quantityText.text = "";
                quantityText.gameObject.SetActive(false);
            }
        }
        
        private void UpdateBackgroundColor()
        {
            if (slotBackground != null)
            {
                slotBackground.color = _isSelected ? selectedColor : normalColor;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotClicked?.Invoke(_slotIndex);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotBackground != null && !_isSelected)
                slotBackground.color = hoverColor;
            OnSlotHovered?.Invoke(_slotIndex);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            UpdateBackgroundColor();
        }
        
        public bool IsEmpty()
        {
            return _slotData == null || _slotData.itemData == null;
        }
        
        public InventorySlot GetSlotData()
        {
            return _slotData;
        }
        
        public int GetSlotIndex()
        {
            return _slotIndex;
        }
    }
}