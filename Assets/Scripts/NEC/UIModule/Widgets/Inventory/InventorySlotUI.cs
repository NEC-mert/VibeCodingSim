using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NEC.UIModule.Common;
using NEC.GameModule.Player.Inventory;
using System;

namespace NEC.UIModule.Widgets.Inventory
{
    public class InventorySlotUI : UIBase, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI Components")]
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text quantityText;
        
        [Header("Visual Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = Color.yellow;
        [SerializeField] private Color emptyColor = Color.clear;
        
        private InventorySlot _slotData;
        private int _slotX, _slotY;
        
        public event Action<int, int> OnSlotClicked;
        public event Action<int, int> OnSlotHovered;
        
        public void SetSlotData(InventorySlot slot, int x, int y)
        {
            _slotData = slot;
            _slotX = x;
            _slotY = y;
            UpdateDisplay();
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
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotClicked?.Invoke(_slotX, _slotY);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotBackground != null)
                slotBackground.color = hoverColor;
            OnSlotHovered?.Invoke(_slotX, _slotY);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (slotBackground != null)
                slotBackground.color = normalColor;
        }
        
        public bool IsEmpty()
        {
            return _slotData == null || _slotData.itemData == null;
        }
        
        public InventorySlot GetSlotData()
        {
            return _slotData;
        }
    }
}