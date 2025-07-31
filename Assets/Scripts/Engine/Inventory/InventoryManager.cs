using System;
using System.Collections.Generic;
using UnityEngine;
using GoopGame.Data;

namespace GoopGame.Engine
{
    /// <summary>
    /// - Contains a list of all items (List<InventoryEntry>)
    /// - Firing events about changes - UI listens
    /// - Handles item swapping, stacking, stack splitting
    /// - Public methods for general use - AddNewItem and RemoveItem
    /// - Public methods for UI when dragging and dropping items. 
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {

        // --- Public API (for shop/game interactions) -----------------
        //public bool  AddNewItem(ItemData itemData, int amount) { … }
        //public void  RemoveItem(int slotIndex)                 { … }

        // --- Public API (for UI drag) --------------------
        //public bool  BeginPickup(int slot)    { … }
        //public bool  PlaceHeld(int slot)      { … }
        //public bool  DepositOne(int slot)     { … }
        //public void  CancelHeld()             { … }
        //public bool  SplitStack(int slot)     { … }
        //public bool  TryPlaceInWorld(int slot){ … }  // TODO


        private List<InventoryEntry> _inventory;

        //Empty entry: used instead of "null" to fill the inventory-list with empty slots.
        public static readonly InventoryEntry Empty = new InventoryEntry(null, 0);

        [SerializeField]
        private int _slotAmount = 15;

        // --- Held stack state
        //When an inventoryItem is dragged, the move should be reflected in the manager.
        private InventoryEntry _heldEntry = null;
        public InventoryEntry HeldEntry => _heldEntry;
        private int _heldOrigin = -1;


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


        // --- Initialization ----

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
            OnInventoryInitialized?.Invoke(_slotAmount);                    //Update UI
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



        // --- Public API - General Use ---

        /// <summary>
        /// Called elsewhere in the game to add an item to the Inventory
        /// - Instantiates InventoryEntry and finds a new slot for it.
        /// </summary>
        /// <returns>True if successfully added, false if no space</returns>
        public bool AddNewItem(ItemData itemData, int amount)
        {

            // ---- 1) Checking if there is enough space before adding ----
            // a) Stackable items: use CanMergeStack to check
            if (itemData.Stackable)
            {
                if (!CanMergeStack(itemData, amount))
                    return false;                       // not enough total space
            }
            // b) Non-stackables: count empty slots
            else
            {
                int empty = 0;
                for (int i = 0; i < _inventory.Count; i++)
                    if (_inventory[i] == Empty) empty++;

                if (empty < amount)
                    return false;                       // not enough individual slots
            }

            // ---- 2) Place Items (guaranteed to fit) ----
            //Trying to merge item into existing stacks:
            if (itemData.Stackable)
            {
                InventoryEntry newEntry = new InventoryEntry(itemData, amount);
                MergeStack(newEntry, amount);
            }
            //If the item isn't stackable, find empty entries for the items:
            else
            {
                int toAdd = amount;
                for (int i = 0; i < _inventory.Count && toAdd > 0; i++)
                {
                    if (_inventory[i] == Empty)
                    {
                        _inventory[i] = new InventoryEntry(itemData, 1);
                        OnSlotChanged?.Invoke(i, _inventory[i]);
                        toAdd--;
                    }
                }
            }

            PrintInventory();   //for testing
            return true;
        }

        //Remove InventoryEntry from list based on slotIndex value
        public void RemoveItem(int slotIndex)
        {
            Debug.Log($"Removed item at index {slotIndex}");
            _inventory[slotIndex] = Empty;
            OnSlotChanged?.Invoke(slotIndex, _inventory[slotIndex]);
        }


        // --- Player Controlled Inventory Operations ---

        /// <summary>
        /// Moves item out of the inventory into _heldEntry during pickup, and notifies UI
        /// </summary>
        public bool BeginPickup(int slotIndex)
        {
            if (_heldEntry != null)
            {
                Debug.LogWarning("InventoryManager tried to pick up item while already holding another");
                return false;
            }
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogError("Tried to pick up item with invalid Index");
                return false;
            }

            InventoryEntry entry = _inventory[slotIndex];
            if (entry == Empty)
            {
                return false; //nothing to pick up
            }

            _heldEntry = entry;
            _heldOrigin = slotIndex;
            _inventory[slotIndex] = Empty;
            OnSlotChanged?.Invoke(slotIndex, Empty);
            return true;
        }
        


        // ---- Public API - Player controlled item management (UI) ----

        /// <summary>
        /// Attempts to place the remaining held stack into target Index.
        /// Implements move into empty slot, merging stacks, and swapping entries.
        /// On merging stacks, the remainder will return to its original slot.
        /// </summary>
        public bool PlaceHeld(int targetIndex)
        {
            if (_heldEntry == null)
            {
                Debug.LogWarning("Tried to place null-entry from hand");
                return false;
            }
            if (!IsValidIndex(targetIndex)) return false;

            var destination = _inventory[targetIndex];

            // 1) Empty target – simple move
            if (destination == Empty)
            {
                _inventory[targetIndex] = _heldEntry;
                ClearHeld();
                OnSlotChanged?.Invoke(targetIndex, _inventory[targetIndex]);
                return true;
            }

            // 2) Attempt merge
            if (_heldEntry.Stackable &&
            destination.Item == _heldEntry.Item &&
            destination.Amount < destination.MaxStack)
            {
                int spaceLeft = destination.MaxStack - destination.Amount;
                int toMove = Mathf.Min(spaceLeft, _heldEntry.Amount);

                destination.SetAmount(destination.Amount + toMove);
                _heldEntry.SetAmount(_heldEntry.Amount - toMove);

                OnSlotChanged?.Invoke(targetIndex, destination);


                if (_heldEntry.Amount == 0)
                {
                    ClearHeld();
                }
                //If there are items left after merge operation, treat the hold as cancelled (to return the remaining items to inventory)
                else
                {
                    CancelHeld();
                }
                return true;
            }

            // 3) Swap
            _inventory[_heldOrigin] = destination;
            _inventory[targetIndex] = _heldEntry;

            OnSlotChanged?.Invoke(_heldOrigin, destination);
            OnSlotChanged?.Invoke(targetIndex, _heldEntry);

            ClearHeld();
            return true;
        }

        /// <summary>
        /// Cancels the drag: places the held stack back into the inventory, preferring its original slot
        /// </summary>
        public void CancelHeld()
        {
            if (_heldEntry == null) return;

            // 1) Try original slot first (handles merge, move, or swap)
            if (_heldOrigin >= 0 && PlaceHeld(_heldOrigin))
                return; // success – PlaceHeld internally cleared the held entry
                

            // 2) Try to merge into any compatible partial stack
            if (_heldEntry.Stackable)
            {
                MergeIntoPartialStacks(_heldEntry);

                if (_heldEntry.Amount == 0)
                {
                    ClearHeld();
                    return;
                }
            }

            // 3) Find first empty slot
            int emptySlotIndex = _inventory.IndexOf(Empty);
            if (emptySlotIndex >= 0)
            {
                _inventory[emptySlotIndex] = _heldEntry;
                OnSlotChanged?.Invoke(emptySlotIndex, _heldEntry);
                ClearHeld();
                return;
            }

            // No space – this should not happen in normal UI flow
            Debug.LogError("Inventory full: unable to cancel held item; discarding.");
            ClearHeld();

        }


        /// <summary>
        /// Moves exactly one item from the held stack to the target slot if possible.
        /// Returns true if a transfer happened.
        /// </summary>
        public bool DepositOne(int targetIndex)
        {
            if (_heldEntry == null) return false;
            if (!IsValidIndex(targetIndex)) return false;
            if (!_heldEntry.Stackable) return false;
            if (_heldEntry.Amount < 1) return false;

            var destination = _inventory[targetIndex];

            // Case 1: Empty slot – create new stack of 1
            if (destination == Empty)
            {
                _inventory[targetIndex] = new InventoryEntry(_heldEntry.Item, 1);
            }
            // Case 2: Same item & space available – increment
            else if (destination.Item == _heldEntry.Item && destination.Amount < destination.MaxStack)
            {
                destination.SetAmount(destination.Amount + 1);
            }
            //Case 3: Slot occupied by different item.
            else
            {
                return false; // cannot merge here
            }

            // Remove one from the held stack
            _heldEntry.SetAmount(_heldEntry.Amount - 1);
            OnSlotChanged?.Invoke(targetIndex, _inventory[targetIndex]);

            if (_heldEntry.Amount == 0)
                ClearHeld();

            return true;
        }


        /// <summary>
        /// Splits a stack in the inventory into two, and assigns half to a new slot
        /// </summary>
        /// <returns>True if the stack was split, else false</returns>
        public bool SplitStack(int index)
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


        //Called by the UI when player wants to place an item
        public bool TryPlaceInWorld(int index)
        {
            // TODO: implement
            return false;
        }



        // ---- Internal Operations ----
        /// <summary>
        /// Checks to see if a quantity of items merged into existing slots/items will exceed the inventory limit.
        /// </summary>
        /// <returns>True if there is space to merge the stack, false if no space</returns>
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

        /// <summary>
        /// Merges the new InventoryEntry with existing stacks and empty slots.
        /// Should never be used unless CanMergeStack has confirmed there is space.
        /// </summary>
        private void MergeStack(InventoryEntry newEntry, int amount)
        {
            //Loop though every slot in the inventory and add amount to existing stacks.
            MergeIntoPartialStacks(newEntry);   // helper mutates newEntry.Amount

            //Place new stacks of the item into the inventory, until there is nothing left.
            for (int i = 0; i < _inventory.Count && newEntry.Amount > 0; i++)
            {
                if (_inventory[i] == Empty)
                {
                    int stackSize = Mathf.Min(newEntry.Amount, newEntry.MaxStack);
                    _inventory[i] = new InventoryEntry(newEntry.Item, stackSize);
                    newEntry.SetAmount(newEntry.Amount - stackSize);
                    OnSlotChanged?.Invoke(i, _inventory[i]);
                }
            }

            if (newEntry.Amount > 0)
            {
                Debug.LogError($"Item merge unsuccessful: {newEntry.Amount} items were lost");
            }
        }

        private void MergeIntoPartialStacks(InventoryEntry source)
        {
            // Non-stackables cannot be merged; just return the original amount
            if (!source.Stackable)
                return;

            // Loop through every slot and add as much as possible
            for (int i = 0; i < _inventory.Count && source.Amount > 0; i++)
            {
                var entry = _inventory[i];

                // Skip empty slots or mismatching items
                if (entry == Empty || entry.Item != source.Item)
                    continue;

                // Skip if this stack is already full
                if (entry.Amount >= entry.MaxStack)
                    continue;

                // Merge as much as fits here
                int spaceLeft = entry.MaxStack - entry.Amount;
                int toMove    = Mathf.Min(spaceLeft, source.Amount);

                entry.SetAmount(entry.Amount + toMove);
                source.SetAmount(source.Amount - toMove);

                OnSlotChanged?.Invoke(i, entry);
            }
        }


        // ---- Low-level utilities -----
        private bool IsValidIndex(int idx)
        {
            return idx >= 0 && idx < _inventory.Count;
        }

        /// <summary>
        /// Sets the _heldEntry to null
        /// </summary>
        private void ClearHeld()
        {
            _heldOrigin = -1;
            _heldEntry = null;
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
