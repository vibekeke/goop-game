using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoopGame.UI {

public class InventorySlot : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// Changes the parent of the dropped element to self. Swaps items if inventory slot is already occupied.
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableUI draggable = dropped.GetComponent<DraggableUI>(); //Gets the item script
                draggable.parentAfterDrag = transform; //Sets the items parent to self
            }
            else
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableUI droppedDraggable = dropped.GetComponent<DraggableUI>();

                GameObject current = transform.GetChild(0).gameObject;
                DraggableUI currentDraggable = current.GetComponent<DraggableUI>();

                currentDraggable.transform.SetParent(droppedDraggable.parentAfterDrag);
                currentDraggable.transform.localPosition = Vector3.zero;

                droppedDraggable.parentAfterDrag = transform;
            }
            //TODO: Add logic for stackable items (should probably call an InventoryManager)
        }
    }
}