using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class DemoItemFactory : MonoBehaviour
    {
        [Header("References")]
        public InventoryManager _inventoryManager;

        [Header("Test Items")]
        public ItemData _testItemA;
        public ItemData _testItemB;

        public void AddItemA()
        {
            TryAddItem(_testItemA, 1);

        }

        public void AddItemB()
        {
            TryAddItem(_testItemB, 3);
        }

        public void TryAddItem(ItemData item, int amount)
        {
            bool added = _inventoryManager.AddNewItem(item, amount);
            Debug.Log(added
            ? $"✅ Added {item.Name} x{amount}"
            : $"❌ Could not add {item.Name} (inventory full?)");
        }
    }
}
