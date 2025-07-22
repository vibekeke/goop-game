using System;
using System.Collections.Generic;
using UnityEngine;
using GoopGame.Data;

namespace GoopGame.Engine
{
    /// <summary>
    /// WIP Responsible for all management of items in the inventory: 
    /// - Contains a list of all items (List<InventoryEntry>)
    /// - Adding and removing items
    /// - Checking for space, assigning slots
    /// - Firing Events about changes. 
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        private List<InventoryEntry> _inventory = new();

        [SerializeField]
        private int _slotAmount;


        // --- EVENTS ---

        //OnInventoryInitialized(int _slotAmount)
        // Called when the inventory is first created or loaded (e.g., from save)
        public event Action<int> OnInventoryInitialized;

        //OnInventoryChanged(List<InventoryEntry> _inventory)
        // Called when the entire inventory changes (bulk update, save load, etc.)
        public event Action<List<InventoryEntry>> OnInventoryChanged;

        //OnSlotChanged(int SlotIndex, InventoryEntry entry)
        // Called when a single slot is changed (new item added, replaced, or amount changed)
        public event Action<int, InventoryEntry> OnSlotChanged;

        //OnItemRemoved(int SlotIndex)
        // Called when an item is removed from a slot entirely
        public event Action<int> OnItemRemoved;


        // --- Inventory Management ----

        //Initialize inventory slots and inventory items.
        public void Start()
        {
            OnInventoryInitialized?.Invoke(_slotAmount);
            LoadInventory();
        }

        private void LoadInventory()
        {
            OnInventoryChanged?.Invoke(_inventory);
        }

        //Called by game when a new item is added
        // - Instantiates InventoryEntry and finds a new slot for it.
        // - Returns false if there was no space for it
        public bool AddNewItem(ItemData newItemData)
        {
            //
            return false;
        }

        //Remove InventoryEntry from list based on slotIndex value
        public void RemoveItem(int _slotIndex)
        {
            //
            OnItemRemoved?.Invoke(_slotIndex);
        }
    }


}
