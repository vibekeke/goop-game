using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// 'Recipe' for initializing a given item into the inventory, and what gameobject it can spawn into the world.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "GoopGame/Items/Create new ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("UI information")]
        public Sprite Icon;             //item icon to be displayed in the inventory UI

        public string Description;      //description of the item to be displayed in the UI

        [Header("Gameplay and logic")]

        public ItemType Type;           //item type for sorting purposes.

        public bool Stackable;

        public GameObject WorldPrefab;  //prefab for gameobject that can be placed into the world/terrarium

    }

    /// <summary>
    /// Description of what type of item it is in case we want to sort the inventory by category later.
    /// </summary>
    public enum ItemType
    {
        PlantSeed,
        Decoration,
        Goop,
        Food
    }

}

