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
        //Hooking up UI elements in the editor so we can change them :3
        [Header("UI references")]
        [SerializeField]
        private Image _image;                  //Reference to the image renderer

        [SerializeField]
        private TextMeshProUGUI _amountText;   //Reference to the text renderer

        //TODO: I plan to have a description window as well :)

        [Header("Public variables")]
        public ItemData itemData;               //Reference to the ScriptableObject 'recipe' for item creation
        public int amount = 1;


        public void Start() //TODO: Change to Init()
        {
            //TODO: Use Parameter to fetch the correct InventoryEntry
            //itemData = entry.recipe;
            //amount = entry.amount;
            UpdateUI();
        }

        //Updates all of the visual elements in the InventoryItem gameobject to match ItemData recipe.
        public void UpdateUI()
        {
            _image.sprite = itemData.Icon;

            //Set the amountText to current amount, or empty string if theres only 1 of the item
            _amountText.text = amount > 1 ? amount.ToString() : string.Empty;

        }
    }
}
