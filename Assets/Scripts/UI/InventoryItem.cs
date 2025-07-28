using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GoopGame.Utility;
using GoopGame.Data;
using GoopGame.Engine;
using TMPro;

namespace GoopGame.UI
{

    /// <summary>
    /// Script for the InventoryItem Prefab used in the UI.
    /// Uses ItemData scriptable object to display the correct images and text.
    /// </summary>
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField]
        private Image _image;                  //Reference to the image renderer

        [SerializeField]
        private TextMeshProUGUI _amountText;   //Reference to the text renderer

        private InventoryUI _inventoryUI;

        [Header("UI-information (temp)")]
        private ItemData _itemData;               //Reference to the ScriptableObject 'recipe' for item creation
        private string _displayName;
        private string _displayDescription;
        private int _displayAmount = -1;

        //static flag for dragging logic
        private static InventoryItem s_CurrentDrag;
        public static InventoryItem CurrentDrag => s_CurrentDrag;


        /// --- Drag and Drop ---
        [HideInInspector]
        public int ParentIndex;
        public Transform ParentBeforeDrag;   //holds the UI-slot the item should snap to

        private Transform _cacheRoot;       //since we access the root several times, and don't expect this to change,
                                            //we cache the value to avoid calling .GetRoot() several times.

        private Transform _root
        {           //property field for root, to check if its cached.
            get
            {
                if (_cacheRoot == null)
                    _cacheRoot = transform.root;
                return _cacheRoot;
            }
        }


        public void Init(int slotIndex, InventoryEntry entry, InventoryUI ui)
        {
            _itemData = entry.Item;
            _displayName = _itemData.Name;
            _displayDescription = _itemData.Description;

            _displayAmount = entry.Amount;
            _inventoryUI = ui;
            ParentIndex = slotIndex;
            UpdateUI();
        }


        //Updates all of the visual elements in the InventoryItem gameobject to match ItemData recipe.
        public void UpdateUI()
        {
            _image.sprite = _itemData.Icon;

            //Set the amountText to current amount, or empty string if theres only 1 of the item
            _amountText.text = _displayAmount > 1 ? _displayAmount.ToString() : string.Empty;

            //TODO: Hover over item to see its name and description
        }
        
        //Used during right-click item transfer. Called by the target InventorySlot.
        public void DecreaseDisplayAmount()
        {
            _displayAmount--;
            if (_displayAmount <= 0)
            {
                // Nothing left in hand; let the drag icon disappear
                Destroy(gameObject);
            }
            else
            {
                UpdateUI();
            }
        }


        // -- Drag and Drop Logic //

        /// <summary>
        /// Movement when player clicks and drags a draggable UI element.
        /// When item is dragged, it is placed on top of the inventory hierarchy for visual purposes.
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                eventData.pointerDrag = null;
                return;                             //Ignore all right/middle mouse button drags. 

            }

            s_CurrentDrag = this;
            Debug.Log("Begin Drag");
            ParentBeforeDrag = transform.parent;
            transform.SetParent(_root);             //Parents the item to the grid instead of the slots
            transform.SetAsLastSibling();           //Sets it at the top of our view
            _image.raycastTarget = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;                             //Ignore all right/middle mouse button drags. 
                
            //Because Canvas Render mode is set to Camera, we need this extra math to calculate pos
            Vector3 mouseScreenPos = Input.mousePosition;

            //Access a cached camera field, since calling Camera.main each frame incurs overhead.
            Vector3 mouseWorldPos = GlobalManager.Camera.ScreenToWorldPoint(mouseScreenPos);
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
            if (eventData.button != PointerEventData.InputButton.Left)
                return;                             //Ignore all right/middle mouse button drags. 
                
            Debug.Log("End Drag");
            transform.SetParent(ParentBeforeDrag);
            transform.localPosition = new Vector3(0, 0, 0);
            _image.raycastTarget = true;

            if (s_CurrentDrag == this)
                s_CurrentDrag = null;
        }

        private void OnDisable()
        {
            if (s_CurrentDrag == this)
                s_CurrentDrag = null;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            // Don’t split stacks while a drag is happening anywhere
            if (s_CurrentDrag != null)
            {
                Debug.Log("Should have blocked the stack split");
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Splitting stack");
                _inventoryUI.RequestSplit(ParentIndex);   
            }
        }
    }
}


