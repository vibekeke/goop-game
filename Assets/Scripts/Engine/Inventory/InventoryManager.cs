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
        private List<InventoryEntry> _inventory;

        //Empty entry: used instead of "null" to fill the inventory-list with empty slots.
        public static readonly InventoryEntry Empty = new InventoryEntry(null, 0);

        [SerializeField]
        private int _slotAmount;


        // --- EVENTS --- 
        //For updating the UI :)

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
            InitializeInventory();
            UpdateInventory();
        }

        /// <summary>
        /// Creates new empty inventory with Empty-entries for each slot. 
        /// Invokes OnInventoryInitialized.
        /// </summary>
        private void InitializeInventory()
        {
            //If the player does not already have an inventory with items in it, create the empty slots!
            if (_inventory == null)
            {
                _inventory = new List<InventoryEntry>(_slotAmount);

                for (int i = 0; i < _slotAmount; i++)
                    _inventory.Add(Empty);
            }
            OnInventoryInitialized?.Invoke(_slotAmount);
        }

        /// <summary>
        /// Passes the saved inventory with all entries to the UI. 
        /// </summary>
        private void UpdateInventory()
        {
            //TOOD: This will hopefully be useful later when we actually have saving and loading of inventory
            OnInventoryChanged?.Invoke(_inventory);
        }

        /// <summary>
        /// Called by game when a new item is added. 
        /// - Instantiates InventoryEntry and finds a new slot for it.
        /// - Returns false if there was no space for it
        /// </summary>
        /// <returns>True if successfully added, false if no space</returns>
        public bool AddNewItem(ItemData itemData, int amount)
        {

            InventoryEntry newEntry = new InventoryEntry(itemData, amount);

            //If the item is stackable, check if the item already exists in the inventory.
            if (itemData.Stackable)
            {
                int stackIndex = LookForStackableSlot(itemData);

                //If the item already exists, try to stack them. If success, return true.
                if (stackIndex != -1)
                {
                    var existing = _inventory[stackIndex];

                    //If the desired stack exceeds the limit, keep looking for an empty slot instead.
                    if (TryMergeStack(existing, newEntry))
                    {
                        Debug.Log("Item successfully stacked");
                        OnSlotChanged?.Invoke(stackIndex, existing);
                        return true;
                    }
                }
            }

            //Find an empty slot
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i] == Empty)                      //if slot is empty
                {
                    _inventory[i] = newEntry;                   //set to newEntry
                    OnSlotChanged?.Invoke(i, _inventory[i]);    //
                    return true;
                }
            }

            //The item couldn't be placed into the inventory. => "Not enough space"
            Debug.Log("Not enough space.");
            return false;
        }

        //Checks if a given itemData matches an existing entry
        private int LookForStackableSlot(ItemData itemData)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                var entry = _inventory[i];
                if (entry != Empty && entry.Item == itemData)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Checks to see if the two entries stacked together will exceed the max stack, and stacks them.
        /// The logic is simple for now: It either stacks them or it doesn't.
        /// </summary>
        private bool TryMergeStack(InventoryEntry oldEntry, InventoryEntry newEntry)
        {
            //if the items added together exceed the stack limit, return false
            if (oldEntry.Amount + newEntry.Amount > oldEntry.GetMaxStack())
                return false;

            //else add the amounts to the oldEntry and return :D
            oldEntry.SetAmount(oldEntry.Amount + newEntry.Amount);
            return true;
        }


        //Remove InventoryEntry from list based on slotIndex value
        public void RemoveItem(int _slotIndex)
        {
            _inventory[_slotIndex] = Empty;
            OnItemRemoved?.Invoke(_slotIndex);
        }


        //Called by the UI when player drops an item onto another item
        public void HandleDrop(int from, int to)
        {
            //
        }

        //Called by the UI when player wants to place an item
        public void TryPlaceItem(int index)
        {
            //
        }
    }
}
