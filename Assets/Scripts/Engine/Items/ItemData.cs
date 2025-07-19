using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// 'Recipe' for initializing a given item into the inventory, and what gameobject it can spawn into the world.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("UI information")]
        public Sprite icon; //item image displayed in the inventory UI

        public ItemType type;

        public string description;

        [Header("Gameplay and logic")]
        public bool stackable;
        public GameObject worldPrefab;

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

