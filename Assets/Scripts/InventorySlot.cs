using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>(); //Gets the item script
            draggableItem.parentAfterDrag = transform; //Sets the items parent to self
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem droppedDraggable = dropped.GetComponent<DraggableItem>();

            GameObject current = transform.GetChild(0).gameObject;
            DraggableItem currentDraggable = current.GetComponent<DraggableItem>();

            currentDraggable.transform.SetParent(droppedDraggable.parentAfterDrag);
            currentDraggable.transform.localPosition = Vector3.zero;

            droppedDraggable.parentAfterDrag = transform;
        }
    }
}
