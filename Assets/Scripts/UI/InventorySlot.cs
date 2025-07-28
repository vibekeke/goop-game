using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoopGame.UI {

    public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public int slotIndex;           //Each slots knows its own index for easier communication with InventoryManager
        private InventoryUI _inventoryUI;

        public void Init(int index, InventoryUI ui)
        {
            slotIndex = index;
            _inventoryUI = ui;
        }

        public event Action<int, int> OnSlotDrop; //from index, to index

        /// <summary>
        /// Registers dropped InventoryItem, fetches it's own index and fires a signal to
        /// InventoryUI.cs featuring the fromIndex (where item originates) and toIndex (itself)
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            //Ignore right and middle mouse button dragging events.
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

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

            //Set the inventoryItem to current slot for visual purposes
            inventoryItem.ParentBeforeDrag = transform;
            OnSlotDrop?.Invoke(fromIndex, toIndex);

            //Destroy the dropped item GameObject to prevent duplicate children
            StartCoroutine(DestroyNextFrame(droppedItem));

        }

        /// <summary>
        /// Requests an item transfer from a held stack onto the inventory slot (transfer of 1 item).
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Only care about right clicks while something is being dragged
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (InventoryItem.CurrentDrag == null) return;

            Debug.Log("Inventory Slot requested transferOne");
            int fromIndex = InventoryItem.CurrentDrag.ParentIndex;
            int toIndex = slotIndex;

            // Ask UI → Manager to move ONE item
            bool ok = _inventoryUI.RequestTransferOne(fromIndex, toIndex);

            // If it worked, shrink the number on the icon in hand
            if (ok)
                InventoryItem.CurrentDrag.DecreaseDisplayAmount();
        }


        private IEnumerator DestroyNextFrame(GameObject obj)
        {
            yield return null;   // allow OnEndDrag to fire
            Destroy(obj);
        }
    }
}