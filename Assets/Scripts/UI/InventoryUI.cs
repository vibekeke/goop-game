using System;
using System.Collections.Generic;
using UnityEngine;
using GoopGame.Data;
using GoopGame.Engine;

namespace GoopGame.UI
{
    /// <summary>
    /// Handles visual updates for the inventory UI.
    /// - Listens to InventoryManager events
    /// - Spawns slots and InventoryItem visuals
    /// - Updates or clears slots as needed
    /// - Forwards requests/calls from InventorySlot and InventoryItem to the InventoryManager
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InventoryManager _inventoryManager; //For listening to events only. We want dumb UI :)
        [SerializeField]
        private Transform _gridContainer;           //The grid to which we will spawn slots and items.
        [SerializeField]
        private GameObject _slotPrefab;
        [SerializeField]
        private GameObject _inventoryItemPrefab;
        [SerializeField]
        private Transform _cursorRoot;              //The Canvas where a dragged item should live for visual purposes.


        // ----
        private List<InventorySlot> _slots = new();

        // Exposed helpers for InventoryItem / InventorySlot => InventoryManager
        public Transform CursorRoot => _cursorRoot;
        public bool HasHeldEntry => _inventoryManager.HeldEntry != null;

        public bool TryPickUp(int index) => _inventoryManager.BeginPickup(index);
        public void CancelHeld() => _inventoryManager.CancelHeld();
        public bool TryPlaceHeld(int index) => _inventoryManager.PlaceHeld(index);
        public bool TryDepositOne(int index) => _inventoryManager.DepositOne(index);
        public bool TrySplitStack(int index) => _inventoryManager.SplitStack(index);


        void Start()
        {
            if (_inventoryManager == null) {
                Debug.LogError("InventoryManager reference not assigned in InventoryUI.");
            }

            _inventoryManager.OnInventoryInitialized += InitSlots;
            _inventoryManager.OnInventoryChanged += UpdateAll;
            _inventoryManager.OnSlotChanged += UpdateSlot;
        }

        /// <summary>
        /// Initializes the grid with a specific amount of slots.
        /// </summary>
        public void InitSlots(int slotAmount)
        {
            //Clearing out the previous stuff so we don't get dupes
            ClearAllItems();

            _slots.Clear();

            for (int i = 0; i < slotAmount; i++)
            {
                GameObject slotGO = Instantiate(_slotPrefab, _gridContainer);   //Instantiates GameObject
                InventorySlot slot = slotGO.GetComponent<InventorySlot>();                //Fetches the InventorySlot script
                slot.Init(i, this);                                                 //Initializes slot with index
                _slots.Add(slot);                                               //Adds slot to list of slots.
            }
        }

        /// <summary>
        /// Updates all slots with the correct InventoryEntry instance
        /// </summary>
        public void UpdateAll(List<InventoryEntry> inventory)
        {
            for (int i = 0; i < inventory.Count; i++)
                UpdateSlot(i, inventory[i]);
        }

        /// <summary>
        /// Instantiates the correct InventoryItem for a single slot. This is called often.
        /// </summary>
        public void UpdateSlot(int slotIndex, InventoryEntry entry)
        {
            // if this slot is currently being dragged, ignore the redraw ---
            if (InventoryItem.CurrentDrag != null &&
                InventoryItem.CurrentDrag.ParentIndex == slotIndex && 
                entry == InventoryManager.Empty)
            {
                // The drag icon will take care of its own count display
                Debug.Log($"Item Redraw Ignored at slot {slotIndex}");
                return;
            }

            InventorySlot slot = _slots[slotIndex];

            // Step 1: Clear existing item visual
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }

            //If the new entry is empty, no further action necessary
            if (entry == InventoryManager.Empty)
                return;

            //Instantiate new InventoryItem based on itemData :D
            GameObject itemGO = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem itemUI = itemGO.GetComponent<InventoryItem>();
            itemUI.Init(slotIndex, entry, this);
        }

        /// <summary>
        /// Deletes the InventoryItem gameobject and its children
        /// </summary>
        public void ClearSlot(int slotIndex)
        {
            InventorySlot slot = _slots[slotIndex];

            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);
        }

        /// <summary>
        /// Erases all InventoryItems from the grid
        /// </summary>
        public void ClearAllItems()
        {
            foreach (Transform child in _gridContainer)
                Destroy(child.gameObject);
        }

    }
}
