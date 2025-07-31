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
    /// Handles Player inputs onto the item, for swapping, merging and stacking purposes.
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
        public Transform _root;
        private Camera _camera;

        public void Awake()
        {
            _camera = GlobalManager.Camera;
        }
        
        public void Init(int slotIndex, InventoryEntry entry, InventoryUI ui)
        {
            _itemData = entry.Item;
            _displayName = _itemData.Name;
            _displayDescription = _itemData.Description;

            _displayAmount = entry.Amount;
            _inventoryUI = ui;
            ParentIndex = slotIndex;
            _root = _inventoryUI.CursorRoot;
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

        /// <summary>
        /// Used during right-click item transfer. Called by the target InventorySlot.
        /// </summary>
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
            //Ignore all right/middle mouse button drags. 
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                eventData.pointerDrag = null;
                return;

            }

            Debug.Log("Begin Drag");
            s_CurrentDrag = this;

            //Ask UIManager -> InventoryManager to lift the stack
            bool held = _inventoryUI.TryPickUp(ParentIndex);
            if (!held)
            {
                eventData.pointerDrag = null;
                s_CurrentDrag = null;
                return;
            }

            //Places the InventoryItem on top of the hierarchy for visual purposes
            transform.SetParent(_root);             //Parents the item to the grid instead of the slots
            transform.SetAsLastSibling();           //Sets it at the top of our view
            transform.position = Input.mousePosition;
            _image.raycastTarget = false;
        }

        /// <summary>
        /// Updates the visual of the item being dragged. UI-only operation
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;                             //Ignore all right/middle mouse button drags. 

            if (s_CurrentDrag != this) return;      //Inputs are irrelevant for all items that are not currently being dragged.

            //Because Canvas Render mode is set to Camera, we need this extra math to calculate pos
            Vector3 mouseScreenPos = Input.mousePosition;

            //Access a cached camera field, since calling Camera.main each frame incurs overhead.
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
            //Set the z position to be equal to the canvas (root object),
            //since it already was a child of the canvas, the z position remains the same.
            mouseWorldPos.z = _root.position.z;

            transform.position = mouseWorldPos;
        }

        /// <summary>
        /// When drag ends, the s_CurrentDrag flag is reset to null
        /// Calls inventory with the correct operation, based on what inputs occurred.
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Ended Drag");

            if (eventData.button != PointerEventData.InputButton.Left)
                return;                             //Ignore all right/middle mouse button drags. 

            if (s_CurrentDrag != this) return;      //Inputs are irrelevant for all items that are not currently being dragged.

            //Did we release over a slot?
            InventorySlot slot = eventData.pointerCurrentRaycast.gameObject
                            ?.GetComponentInParent<InventorySlot>();

            s_CurrentDrag = null;
            bool placed = false;

            //Calling the InventoryManager about the change!
            if (slot != null)
            {
                placed = _inventoryUI.TryPlaceHeld(slot.slotIndex);
            }
            if (!placed) _inventoryUI.CancelHeld();

            Destroy(gameObject);        //The Item always disappears on end-drag, InventoryManager handles the rest.
        }

        private void OnDisable()
        {
            if (s_CurrentDrag == this)
                s_CurrentDrag = null;
        }

        /// <summary>
        /// When player right clicks while dragging an item - call InventoryUI to try to deposit one.
        /// When a player right clicks with no held item - call InventoryUI to try to split divide the stack in half.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // If an item is currently being dragged...
            if (s_CurrentDrag != null)
            {
                // Treat a right-click as “deposit one here”
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    bool ok = _inventoryUI.TryDepositOne(ParentIndex);
                    if (ok && InventoryItem.CurrentDrag != null)
                        InventoryItem.CurrentDrag.DecreaseDisplayAmount(); // keep icon count in sync
                }
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Splitting stack");
                _inventoryUI.TrySplitStack(ParentIndex);
            }
        }
    }
}


