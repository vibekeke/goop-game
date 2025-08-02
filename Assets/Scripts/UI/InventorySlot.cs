using UnityEngine;
using UnityEngine.EventSystems;

namespace GoopGame.UI {

    /// <summary>
    /// Visual container for InventoryItems. Can also register right click so player can deposit items into an empty slot.
    /// </summary>
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public int slotIndex;           //Each slots knows its own index for easier communication with InventoryManager
        private InventoryUI _inventoryUI;


        public void Init(int index, InventoryUI ui)
        {
            slotIndex = index;
            _inventoryUI = ui;
        }


        /// <summary>
        /// Requests an item transfer from a held stack onto the inventory slot (transfer of 1 item).
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Only care about right clicks while something is being dragged
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (!_inventoryUI.HasHeldEntry) return;

            // Ask UI → Manager to move ONE item
            bool deposited = _inventoryUI.TryDepositOne(slotIndex);

            // If it worked, shrink the number on the icon in hand
            if (deposited)
                InventoryItem.CurrentDrag.DecreaseDisplayAmount();
        }
    }
}