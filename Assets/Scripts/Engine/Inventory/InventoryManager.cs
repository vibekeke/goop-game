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
        private int _slotAmount = 10;


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
            Debug.Log($"Inventory Initialized with {_slotAmount} slots");
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

            //If the item is stackable, and there is space, try to merge them.

            InventoryEntry newEntry = new InventoryEntry(itemData, amount);

            if (itemData.Stackable && CanMergeStack(itemData, amount))
            {
                Debug.Log("Should have merged");
                MergeStack(newEntry, amount);
                PrintInventory();
                return true;
            }

            //If the item isn't stackable, just add it straight to the inventory :)

            //Find an empty slot
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i] == Empty)                      //if slot is empty
                {
                    _inventory[i] = newEntry;                   //set to newEntry
                    OnSlotChanged?.Invoke(i, _inventory[i]);    //
                    PrintInventory();
                    return true;
                }
            }

            //The item couldn't be placed into the inventory. => "Not enough space"
            Debug.Log("Not enough space.");
            return false;
        }


        /// <summary>
        /// Checks to see if the two entries stacked together will exceed the inventory limit.
        /// </summary>
        private bool CanMergeStack(ItemData itemData, int amount)
        {
            int StackableSpace = 0;
            int emptySlots = 0;

            //Goes through inventory and checks 
            // (1) how many slots we have, and 
            // (2) how many spaces are left in the existing stacks of the same itemtype.
            for (int i = 0; i < _inventory.Count; i++)
            {
                var entry = _inventory[i];

                if (entry == Empty)
                {
                    emptySlots++;
                }
                else if (entry.Item == itemData)
                {
                    StackableSpace += entry.MaxStack - entry.Amount;
                }
            }

            //Tallies up all of the empty space in the inventory :)
            int TotalStackableSpace = StackableSpace + (emptySlots * itemData.MaxStack);
            return TotalStackableSpace >= amount;

        }

        //Merges the new InventoryEntry with existing stacks and empty slots
        private void MergeStack(InventoryEntry newEntry, int amount)
        {
            //Loop though every slot in the inventory and add amount to existing stacks.
            for (int i = 0; i < _inventory.Count && newEntry.Amount > 0; i++)
            {
                var entry = _inventory[i];

                // Skip empty or mismatched items
                if (entry == Empty || entry.Item != newEntry.Item)
                    continue;

                // If this stack is full, skip it
                if (entry.Amount >= entry.MaxStack)
                    continue;

                // Merge as much as we can into this stack
                int spaceLeft = entry.MaxStack - entry.Amount;
                int toMerge = Mathf.Min(spaceLeft, newEntry.Amount);

                entry.SetAmount(entry.Amount + toMerge);
                newEntry.SetAmount(newEntry.Amount - toMerge);

                OnSlotChanged?.Invoke(i, entry);
            }

            //Place new stacks of the item into the inventory, until there is nothing left.
            for (int i = 0; i < _inventory.Count && newEntry.Amount > 0; i++)
            {
                if (_inventory[i] == Empty)
                {
                    int stackSize = Mathf.Min(newEntry.Amount, newEntry.MaxStack);
                    _inventory[i] = new InventoryEntry(newEntry.Item, stackSize); //Will this cause wierdness??
                    newEntry.SetAmount(newEntry.Amount - stackSize);
                    OnSlotChanged?.Invoke(i, _inventory[i]);
                }
            }

            if (newEntry.Amount > 0)
            {
                Debug.LogError($"Item merge unsuccessful: {newEntry.Amount} items were lost");
            }
        }


        //Remove InventoryEntry from list based on slotIndex value
        public void RemoveItem(int _slotIndex)
        {
            Debug.Log("Should have removed it...");
            _inventory[_slotIndex] = Empty;
            OnSlotChanged?.Invoke(_slotIndex, _inventory[_slotIndex]);
        }


        //Called by the UI when player drops an item onto another item
        public void HandleDrop(int fromIndex, int toIndex)
        {
            Debug.Log($"Inventory Manager registered drop from index {fromIndex} to {toIndex}");
            InventoryEntry droppedEntry = _inventory[fromIndex];
            InventoryEntry existingEntry = _inventory[toIndex];

            if (existingEntry == Empty)
            {
                _inventory[toIndex] = droppedEntry;
                _inventory[fromIndex] = Empty;

                Debug.Log($"Tried moving item from {fromIndex} to {toIndex}");
                OnSlotChanged?.Invoke(fromIndex, Empty);
                OnSlotChanged?.Invoke(toIndex, droppedEntry);
                return;
            }

            //Stack Logic
            if (droppedEntry.Stackable && droppedEntry.Item == existingEntry.Item)
            {
                //Stacking is only relevant if existingEntry has space
                if (existingEntry.Amount < existingEntry.MaxStack)
                {
                    int spaceLeft = existingEntry.MaxStack - existingEntry.Amount;

                    //Add as much as possible to the existing stack
                    int toMove = Mathf.Min(spaceLeft, droppedEntry.Amount);
                    existingEntry.SetAmount(existingEntry.Amount + toMove);
                    droppedEntry.SetAmount(droppedEntry.Amount - toMove);

                    //if there is nothing left in the dropped stack, clear that entry
                    if (droppedEntry.Amount == 0)
                    {
                        _inventory[fromIndex] = Empty;
                    }

                    OnSlotChanged?.Invoke(fromIndex, _inventory[fromIndex]);
                    OnSlotChanged?.Invoke(toIndex, existingEntry);
                    return;
                }
            }


            //Swap Logic
            if (existingEntry != Empty)
            {
                _inventory[toIndex] = droppedEntry;
                _inventory[fromIndex] = existingEntry;

                OnSlotChanged?.Invoke(fromIndex, existingEntry);
                OnSlotChanged?.Invoke(toIndex, droppedEntry);
                return;
            }


            //If nothing has changed yet...
            _inventory[fromIndex] = droppedEntry;
            OnSlotChanged?.Invoke(fromIndex, droppedEntry);
        }

        public bool TrySplitStack(int index)
        {
            InventoryEntry entry = _inventory[index];
            if (entry == Empty || entry.Amount < 2)
                return false;

            int half = entry.Amount / 2;

            //Find an empty slot
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i] == Empty)
                {
                    entry.SetAmount(entry.Amount - half);                  //Shrink original stack
                    _inventory[i] = new InventoryEntry(entry.Item, half);   //Create new

                    OnSlotChanged?.Invoke(index, entry);
                    OnSlotChanged?.Invoke(i, _inventory[i]);
                    return true;
                }
            }

            //The item couldn't be placed into the inventory. => "Not enough space"
            Debug.Log("Not enough space to split stack");
            return false;
        }

        /// <summary>
        /// Removes one from an existing stack and adds it to another - if possible.
        /// Used when an item is dragged + right click over an inventory slot.
        /// - When a drag begins, the source slot is marked as “drag-owned” so the UI never regenerates an icon there; 
        /// every successful TryTransferOne call then updates the icon in the player’s hand instead of redrawing the hidden source slot.
        /// </summary>
        public bool TryTransferOne(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return false;

            InventoryEntry source = _inventory[fromIndex];
            InventoryEntry destination = _inventory[toIndex];

            if (!source.Stackable) return false;                        //cannot split or merge a non-stackable stack
            if (source == Empty || source.Amount < 1) return false;     //(source should never really be empty but it's nice to double check)


            //case 1: Target slot is empty
            if (destination == Empty)
            {
                _inventory[toIndex] = new InventoryEntry(source.Item, 1);
            }
            //case 2: Target slot has the same stackable item
            else if (destination.Item == source.Item && destination.Amount < destination.MaxStack)
            {
                destination.SetAmount(destination.Amount + 1);
            }
            //case 3: Target slot is occupied, cannot merge.
            else
            {
                return false;
            }

            source.SetAmount(source.Amount - 1);

            if (source.Amount == 0)
            {
                _inventory[fromIndex] = Empty;
            }

            OnSlotChanged?.Invoke(fromIndex, _inventory[fromIndex]);
            OnSlotChanged?.Invoke(toIndex, _inventory[toIndex]);
            return true;
        }

        //Called by the UI when player wants to place an item
        public void TryPlaceItem(int index)
        {
            //
        }


        private void PrintInventory()
        {
            Debug.Log("Inventory contents:");

            string log = "📦 Inventory: ";

            for (int i = 0; i < _inventory.Count; i++)
            {
                var entry = _inventory[i];
                if (entry == Empty)
                    log += $"[{i}: Empty] ";
                else
                    log += $"[{i}: {entry.Item.Name} x{entry.Amount}] ";
            }

            Debug.Log(log);
        }
    }
}
