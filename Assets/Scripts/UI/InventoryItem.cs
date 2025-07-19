using UnityEngine;
using UnityEngine.UI;
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
        [Header("UI references")]
        [SerializeField]
        private Image _image;   //Reference to the image renderer

        [SerializeField]
        private TextMeshProUGUI _amountText;   //Reference to the text renderer

        //Plan to have a description window as well :)

        [Header("Public variables")]
        public ItemData itemData; //Reference to the ScriptableObject 'recipe' for item creation
        public int amount = 1;



        public void Start()
        {
            UpdateUI();
        }


        public void UpdateUI()
        {
            _image.sprite = itemData.icon;
            _amountText.text = amount.ToString();
        }

    }
}
