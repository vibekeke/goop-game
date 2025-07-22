using UnityEngine;
using UnityEngine.UI;
using GoopGame.Data;
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
        public ItemData itemData;               //Reference to the ScriptableObject 'recipe' for item creation

        //IMPORTANT!! Almost none of these variables will be public later, they are only public now
        //for testing purposes!
        private string displayName;
        private string displayDescription;
        private int displayAmount = 1;



        public void Init(InventoryEntry entry)
        {
            itemData = entry.Item;
            displayName = itemData.Name;
            displayDescription = itemData.Description;

            displayAmount = entry.Amount;
            UpdateUI();
        }


        //Updates all of the visual elements in the InventoryItem gameobject to match ItemData recipe.
        public void UpdateUI()
        {
            _image.sprite = itemData.Icon;

            //Set the amountText to current amount, or empty string if theres only 1 of the item
            _amountText.text = displayAmount > 1 ? displayAmount.ToString() : string.Empty;

            //TODO: Hover over item to see its name and description
        }
    }
}
