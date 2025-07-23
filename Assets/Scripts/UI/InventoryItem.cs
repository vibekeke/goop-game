using UnityEngine;
using UnityEngine.UI;
using GoopGame.Data;
using GoopGame.Engine;
using TMPro;

namespace GoopGame.UI
{
    
    /// <summary>
    /// Script for the InventoryItem Prefab used in the UI.
    /// Uses ItemData scriptable object to display the correct images and text.
    /// </summary>
    public class InventoryItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private Image _image;                  //Reference to the image renderer

        [SerializeField]
        private TextMeshProUGUI _amountText;   //Reference to the text renderer



        [Header("UI-information (temp)")]
        private ItemData _itemData;               //Reference to the ScriptableObject 'recipe' for item creation
        private string _displayName;
        private string _displayDescription;
        private int _displayAmount = -1;


        public void Init(InventoryEntry entry)
        {
            _itemData = entry.Item;
            _displayName = _itemData.Name;
            _displayDescription = _itemData.Description;

            _displayAmount = entry.Amount;
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
    }
}
