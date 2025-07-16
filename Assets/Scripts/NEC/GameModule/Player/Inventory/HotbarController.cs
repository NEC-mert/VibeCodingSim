using UnityEngine;
using System;

namespace NEC.GameModule.Player.Inventory
{
    public class HotbarController : MonoBehaviour
    {
        [Header("Hotbar Settings")]
        [Tooltip("The number of slots in the hotbar.")]
        [SerializeField] private int hotbarSize = 6;
        
        [Header("Item Holder")]
        [Tooltip("The transform parented to the player's camera that will hold the item model.")]
        [SerializeField] private Transform itemHolder;
        
        private InventorySlot[] _hotbarSlots;
        private int _activeSlotIndex;
        private GameObject _currentHeldItem;
        
        public event Action<int> OnActiveSlotChanged;
        public event Action OnHotbarUpdated;
        
        public int ActiveSlotIndex => _activeSlotIndex;
        public int HotbarSize => hotbarSize;
        
        private void Awake()
        {
            InitializeHotbar();
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void InitializeHotbar()
        {
            _hotbarSlots = new InventorySlot[hotbarSize];
            for (int i = 0; i < hotbarSize; i++)
            {
                _hotbarSlots[i] = new InventorySlot();
            }
        }
        
        private void HandleInput()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                if (scroll > 0) 
                    SelectPreviousSlot();
                else 
                    SelectNextSlot();
            }
            
            for (var i = 0; i < hotbarSize && i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectSlot(i);
                }
            }
        }
        
        public void SelectSlot(int newIndex)
        {
            if (newIndex < 0 || newIndex >= hotbarSize) return;
            
            var oldIndex = _activeSlotIndex;
            _activeSlotIndex = newIndex;
            
            UpdateHeldItem();
            OnActiveSlotChanged?.Invoke(_activeSlotIndex);
            
            var selectedItem = _hotbarSlots[_activeSlotIndex].itemData;
            if (selectedItem)
            {
                Debug.Log($"Selected slot {_activeSlotIndex + 1}: {selectedItem.itemName}");
            }
            else
            {
                Debug.Log($"Selected slot {_activeSlotIndex + 1}: Empty");
            }
        }
        
        public void SelectNextSlot()
        {
            var nextIndex = (_activeSlotIndex + 1) % hotbarSize;
            SelectSlot(nextIndex);
        }
        
        public void SelectPreviousSlot()
        {
            var prevIndex = _activeSlotIndex - 1;
            if (prevIndex < 0) prevIndex = hotbarSize - 1;
            SelectSlot(prevIndex);
        }
        
        private void UpdateHeldItem()
        {
            if (itemHolder == null) return;
            if (_currentHeldItem != null) Destroy(_currentHeldItem);
            
            var itemToHold = _hotbarSlots[_activeSlotIndex].itemData;
            if (itemToHold != null && itemToHold.inHandPrefab != null)
            {
                _currentHeldItem = Instantiate(itemToHold.inHandPrefab, itemHolder);
                _currentHeldItem.transform.localPosition = Vector3.zero;
                _currentHeldItem.transform.localRotation = Quaternion.identity;
            }
        }
        
        public InventorySlot GetHotbarSlot(int index)
        {
            if (index < 0 || index >= hotbarSize)
                return null;

            return _hotbarSlots[index];
        }
        
        public void SetHotbarSlot(int index, InventorySlot slot)
        {
            if (index < 0 || index >= hotbarSize)
                return;
            
            _hotbarSlots[index] = slot ?? new InventorySlot();
            
            if (index == _activeSlotIndex)
            {
                UpdateHeldItem();
            }
            
            OnHotbarUpdated?.Invoke();
        }
        
        public void SwapSlots(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= hotbarSize || indexB < 0 || indexB >= hotbarSize)
                return;
                
            (_hotbarSlots[indexA], _hotbarSlots[indexB]) = (_hotbarSlots[indexB], _hotbarSlots[indexA]);

            if (indexA == _activeSlotIndex || indexB == _activeSlotIndex)
            {
                UpdateHeldItem();
            }
            
            OnHotbarUpdated?.Invoke();
        }
        
        public void ClearSlot(int index)
        {
            if (index < 0 || index >= hotbarSize)
                return;
            
            _hotbarSlots[index] = new InventorySlot();
            
            if (index == _activeSlotIndex)
            {
                UpdateHeldItem();
            }
            
            OnHotbarUpdated?.Invoke();
        }
        
        public bool AddItem(ItemData itemData, int quantity = 1)
        {
            for (var i = 0; i < hotbarSize; i++)
            {
                if (_hotbarSlots[i].itemData == null)
                {
                    _hotbarSlots[i].itemData = itemData;
                    _hotbarSlots[i].quantity = quantity;
                    
                    if (i == _activeSlotIndex)
                    {
                        UpdateHeldItem();
                    }
                    
                    OnHotbarUpdated?.Invoke();
                    return true;
                }
                else if (_hotbarSlots[i].itemData == itemData)
                {
                    _hotbarSlots[i].quantity += quantity;
                    OnHotbarUpdated?.Invoke();
                    return true;
                }
            }
            
            return false;
        }
    }
}