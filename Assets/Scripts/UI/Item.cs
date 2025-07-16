using Codice.Client.BaseCommands.Merge.Xml;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GoopGame.Engine
{
    //Following tutorial by Coco Code
    [CreateAssetMenu(menuName = "Scriptable object/Item")]
    public class Item : ScriptableObject
    {

        [Header("Only gameplay")]
        public TileBase tile; //shows a graphic on the in-game grid (won't be used)
        public ItemType type;
        public ActionType actionType;
        public Vector2Int range = new Vector2Int(5, 4); //won't be used in the inventory itself, but defines some other stuff

        [Header("Only UI")]
        public bool stackable = true;

        [Header("Both")]
        public Sprite image;

    }

    public enum ItemType
    {
        BuildingBlock,
        Tool
    }

    public enum ActionType
    {
        Dig,
        Mine
    }
}
