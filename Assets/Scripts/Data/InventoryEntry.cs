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

        //TODO: If we want to be able to save "living" objects - goops, plants etc with mutable stats, we can add 
        // a variable "customData" or "itemSnapshot" here that saves the in-game variables here. 
        // If there is a better way of doing this lmk, but I have been told we cannot save gameObjects directly
        // because the stats will be lost on closing the game or something.

        public InventoryEntry(ItemData itemData, int amount)
        {
            Item = itemData;
            Amount = amount;
        }

        public void SetAmount(int newAmount)
        {
            Amount = newAmount;
        }

        public string GetName()
        {
            return Item.Name;
        }

        public int GetMaxStack()
        {
            return Item.MaxStack;
        }
    }
}
