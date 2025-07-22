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
        private InventoryManager _inventoryManager;
        [SerializeField]
        private Transform _gridContainer;           //The grid to which we will spawn slots and items.
        [SerializeField]
        private GameObject _slotPrefab;
        [SerializeField]
        private GameObject _inventoryItemPrefab;

        private int _slotAmount;

        void Start()
        {
            if (_inventoryManager == null)
                Debug.LogError("InventoryManager reference not assigned in InventoryUI.");

            _inventoryManager.OnInventoryInitialized += Init;
            _inventoryManager.OnInventoryChanged += UpdateAll;
            _inventoryManager.OnSlotChanged += UpdateSlot;
            _inventoryManager.OnItemRemoved += ClearSlot;
        }

        //Initializes the grid with a specific amount of slots.
        public void Init(int slotAmount)
        {
            _slotAmount = slotAmount;
            //Create all the children :)
        }

        public void UpdateAll(List<InventoryEntry> inventory)
        {
            
        }

        //Updates a given InventoryItem with small stat updates (i.e. a new amount or new description)
        public void UpdateSlot(int slotIndex, InventoryEntry entry)
        {

        }

        //Erases a given InventoryItem from the grid
        public void ClearSlot(int slotIndex)
        {

        }

        //Erases all InventoryItems from the grid
        public void ClearAll()
        {

        }

    }
}
