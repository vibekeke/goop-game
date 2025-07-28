using System;
using System.Collections.Generic;
using UnityEngine;
using GoopGame.Data;
using GoopGame.Engine;

namespace GoopGame.UI
{
    /// <summary>
    /// WIP! 
    /// Handles visual updates for the inventory UI.
    /// - Listens to InventoryManager events
    /// - Spawns slots and InventoryItem visuals
    /// - Updates or clears slots as needed
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private InventoryManager _inventoryManager; //For listening to events only. We want dumb UI :)
        [SerializeField]
        private Transform _gridContainer;           //The grid to which we will spawn slots and items.
        [SerializeField]
        private GameObject _slotPrefab;
        [SerializeField]
        private GameObject _inventoryItemPrefab;

        private List<InventorySlot> _slots = new();

        void Start()
        {
            if (_inventoryManager == null)
                Debug.LogError("InventoryManager reference not assigned in InventoryUI.");

            _inventoryManager.OnInventoryInitialized += InitSlots;
            _inventoryManager.OnInventoryChanged += UpdateAll;
            _inventoryManager.OnSlotChanged += UpdateSlot;
        }

        //Initializes the grid with a specific amount of slots.
        public void InitSlots(int slotAmount)
        {
            //Clearing out the previous stuff so we don't get dupes
            foreach (Transform child in _gridContainer)
                Destroy(child.gameObject);

            _slots.Clear();

            for (int i = 0; i < slotAmount; i++)
            {
                GameObject slotGO = Instantiate(_slotPrefab, _gridContainer);   //Instantiates GameObject
                InventorySlot slot = slotGO.GetComponent<InventorySlot>();                //Fetches the InventorySlot script
                slot.Init(i, this);
                slot.OnSlotDrop += HandleItemDrop;                                                  //Initializes slot with index
                _slots.Add(slot);                                               //Adds slot to list of slots.
            }
        }

        public void UpdateAll(List<InventoryEntry> inventory)
        {
            for (int i = 0; i < inventory.Count; i++)
                UpdateSlot(i, inventory[i]);
        }

        //Updates a given slot with a new InventoryEntry.
        public void UpdateSlot(int slotIndex, InventoryEntry entry)
        {
            // if this slot is currently being dragged, ignore the redraw ---
            if (InventoryItem.CurrentDrag != null &&
                InventoryItem.CurrentDrag.ParentIndex == slotIndex)
            {
                // The drag icon will take care of its own count display
                return;
            }

            InventorySlot slot = _slots[slotIndex];

            // Step 1: Clear existing item visual
            foreach (Transform child in slot.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            //If the new entry is empty, no further action necessary
            if (entry == InventoryManager.Empty)
                return;

            //Instantiate new InventoryItem based on itemData :D
            GameObject itemGO = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem itemUI = itemGO.GetComponent<InventoryItem>();
            itemUI.Init(slotIndex, entry, this);
        }

        //Erases a given InventoryItem from the grid
        public void ClearSlot(int slotIndex)
        {
            InventorySlot slot = _slots[slotIndex];

            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);
        }

        //Erases all InventoryItems from the grid
        public void ClearAll()
        {

        }


        // --- Requesting changes in Inventory --
        public void HandleItemDrop(int fromIndex, int toIndex)
        {
            _inventoryManager.HandleDrop(fromIndex, toIndex);
        }

        public void RequestSplit(int slotIndex)
        {
            //TrySplitStack returns a bool, to be used for visual feedback later.
            _inventoryManager.TrySplitStack(slotIndex);
        }

        public bool RequestTransferOne(int fromIndex, int toIndex)
        {
            //TryTransferOne returns a bool, to be used for visual feedback later.
            return _inventoryManager.TryTransferOne(fromIndex, toIndex);
        }
    }
}
