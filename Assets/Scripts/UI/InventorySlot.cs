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
            //If there is not item in this slot:
            if (transform.childCount == 0)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableUI draggable = dropped.GetComponent<DraggableUI>();    //Gets the item script
                draggable.ParentAfterDrag = transform;                          //Sets the items parent to self
            }
            //If there is an item, swap them:
            else
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableUI droppedDraggable = dropped.GetComponent<DraggableUI>();

                GameObject current = transform.GetChild(0).gameObject;
                DraggableUI currentDraggable = current.GetComponent<DraggableUI>();

                //Set the previous item's parent field to the dropped item to swap them.
                currentDraggable.transform.SetParent(droppedDraggable.ParentAfterDrag);
                currentDraggable.transform.localPosition = Vector3.zero;

                droppedDraggable.ParentAfterDrag = transform;
            }
            //TODO: Add logic for stackable items (should probably call an InventoryManager)
        }
    }
}