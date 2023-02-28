using BepInEx;
using SpaceCraft;
using System.Linq;

namespace PurgeWhenFull
{
    internal class Recyclers
    {
        internal static void Purge()
        {
            UnityEngine.Object.FindObjectsOfType<ActionRecycle>().ToList().ForEach(recycleable =>
            {
                string groupId = recycleable.transform.parent?.parent?.parent?.parent?.GetComponent<WorldObjectAssociated>()?.GetWorldObject()?.GetGroup()?.id;
                if (!groupId.IsNullOrWhiteSpace() && groupId.StartsWith("RecyclingMachine"))
                {
                    Inventory inventory = recycleable.transform.parent?.parent?.parent?.parent?.GetComponent<InventoryAssociated>()?.GetInventory();
                    if (inventory != null && inventory.IsFull()) { recycleable.OnAction(); }
                }
            });
        }
    }
}