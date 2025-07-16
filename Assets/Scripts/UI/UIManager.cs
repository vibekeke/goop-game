using UnityEngine;

namespace GoopGame.UI
{
    /// <summary>
    /// Centralized manager for events triggered by UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public GameObject inventoryPanel;

        /// <summary>
        /// Toggles inventoryPanels active status.
        /// </summary>
        public void ToggleInventory()
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            //Should add cool animations and shit here I guess?
        }
    }

}
