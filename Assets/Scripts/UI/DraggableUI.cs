using GoopGame.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoopGame.UI 
{
    /// <summary>
    /// Script for handling dragging logic in UI elements. 
    /// </summary>
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _image;

        [HideInInspector]
        public Transform ParentAfterDrag;   //holds the UI-slot the item should snap to

        private Transform _cacheRoot;       //since we access the root several times, and don't expect this to change,
                                            //we cache the value to avoid calling .GetRoot() several times.

        private Transform _root {           //property field for root, to check if its cached.
            get { 
                if (_cacheRoot == null) 
                    _cacheRoot = transform.root; 
                return _cacheRoot; 
            } 
        }

        /// <summary>
        /// Movement when player clicks and drags a draggable UI element.
        /// When item is dragged, it is placed on top of the inventory hierarchy for visual purposes
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Begin Drag");
            ParentAfterDrag = transform.parent;
            transform.SetParent(_root);             //Parents the item to the grid instead of the slots
            transform.SetAsLastSibling();           //Sets it at the top of our view
            _image.raycastTarget = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            //Because Canvas Render mode is set to Camera, we need this extra math to calculate pos
            Vector3 mouseScreenPos = Input.mousePosition;

            //Access a cached camera field, since calling Camera.main each frame incurs overhead.
            Vector3 mouseWorldPos = GlobalManager.Camera.WorldToScreenPoint(mouseScreenPos);
            //Set the z position to be equal to the canvas (root object),
            //since it already was a child of the canvas, the z position remains the same.
            mouseWorldPos.z = _root.position.z;

            transform.position = mouseWorldPos;
        }

        /// <summary>
        /// By default, parentAfterDrag remains the same and so the item will snap back to it's original position.
        /// However,  other scripts can change this variable to parent the UI-element to something else.
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End Drag");
            transform.SetParent(ParentAfterDrag);
            transform.localPosition = Vector3.zero;
            _image.raycastTarget = true;
        }
    }
}