using UnityEngine;

namespace GoopGame.Data
{

    /// <summary>
    /// Item entry to be stored in the inventory. 
    /// Should be initialized in the game (shop, pickup, trade) and added to the inventory through the InventoryManager.
    /// </summary>
    [System.Serializable]
    public class InventoryEntry
    {
        public ItemData Item;
        public int Amount;
        public int SlotIndex;

        //TODO: If we want to be able to save "living" objects - goops, plants etc with mutable stats, we can add 
        // a variable "customData" or "itemSnapshot" here that saves the in-game variables here. 
        // If there is a better way of doing this lmk, but I have been told we cannot save gameObjects directly
        // because the stats will be lost on closing the game or something.

        public InventoryEntry(ItemData itemData, int amount, int slot)
        {
            Item = itemData;
            Amount = amount;
            SlotIndex = slot;
        }
    }
}
