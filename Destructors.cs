using BepInEx;
using SpaceCraft;
using System.Linq;

namespace PurgeWhenFull
{
    internal class Destructors
    {
        internal static void Purge()
        {
            UnityEngine.Object.FindObjectsOfType<ActionDestroyInventory>().ToList().ForEach(destroyable =>
            {
                string group = destroyable.transform.parent?.parent?.GetComponent<WorldObjectAssociated>()?.GetWorldObject()?.GetGroup()?.id;
                if (!group.IsNullOrWhiteSpace() && group.StartsWith("Destructor"))
                {
                    Inventory inventory = destroyable.transform.parent?.parent?.GetComponent<InventoryAssociated>()?.GetInventory();
                    if (inventory != null && inventory.IsFull()) { destroyable.OnAction(); }
                }
            });
        }
    }
}