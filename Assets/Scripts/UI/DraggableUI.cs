using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;


namespace GoopGame.UI 
{
    /// <summary>
    /// Script for handling dragging logic in UI elements. 
    /// </summary>
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image image;
        [HideInInspector] public Transform parentAfterDrag; //holds the UI-slot the item should snap to
        
        /// <summary>
        /// Movement when player clicks and drags a draggable UI element.
        /// When item is dragged, it is placed on top of the inventory hierarchy for visual purposes
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Begin Drag");
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root); //Parents the item to the grid instead of the slots
            transform.SetAsLastSibling(); //Sets it at the top of our view
            image.raycastTarget = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Dragging");

            //Because Canvas Render mode is set to Camera, we need this extra math to calculate pos
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            transform.position = mouseWorldPos;
        }

        /// <summary>
        /// By default, parentAfterDrag remains the same and so the item will snap back to it's original position.
        /// However,  other scripts can change this variable to parent the UI-element to something else.
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End Drag");
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
            image.raycastTarget = true;
        }
    }
}