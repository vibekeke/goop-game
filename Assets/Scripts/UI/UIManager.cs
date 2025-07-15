using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryPanel;

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        //Should add cool animations and shit here I guess?
    }
}
