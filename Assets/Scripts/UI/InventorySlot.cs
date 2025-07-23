using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoopGame.UI {

public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;           //Each slots knows its own index for easier communication with InventoryManager

        public void Init(int index)
        {
            slotIndex = index;
        }

        public event Action<int, int> OnSlotDrop; //from index, to index

        /// <summary>
        /// Registers dropped InventoryItem, fetches it's own index and fires a signal to
        /// InventoryUI.cs featuring the fromIndex (where item originates) and toIndex (itself)
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            //Fetches InventoryItem object dropped onto it.
            GameObject droppedItem = eventData.pointerDrag;

            if (droppedItem == null)
                return;

            //Get's the inventoryItem script, for the purpose of getting to- and from-index
            InventoryItem inventoryItem = droppedItem.GetComponent<InventoryItem>();

            if (inventoryItem == null)
            {
                Debug.LogError("Dropped Item does not have an InventoryItem script D:");
                return;
            }

            int fromIndex = inventoryItem.ParentIndex;
            int toIndex = slotIndex;


            if (fromIndex == toIndex)
            {
                return; //Cannot drop item onto itself.
            }

            OnSlotDrop?.Invoke(fromIndex, toIndex);

            Destroy(droppedItem); //Destroy the dropped item GameObject to prevent duplicate children
        }
    }
}