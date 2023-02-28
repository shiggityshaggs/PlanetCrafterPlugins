using BepInEx;
using SpaceCraft;
using System.Linq;

namespace SSPCP_PurgeWhenFull.Classes
{
    internal class Destructors
    {
        internal static void Purge()
        {
            UnityEngine.Object.FindObjectsOfType<ActionDestroyInventory>().ToList().ForEach(destroyable =>
            {
                string groupId = destroyable.transform.parent?.parent?.GetComponent<WorldObjectAssociated>()?.GetWorldObject()?.GetGroup()?.id;
                if (!groupId.IsNullOrWhiteSpace() && groupId.StartsWith("Destructor"))
                {
                    Inventory inventory = destroyable.transform.parent?.parent?.GetComponent<InventoryAssociated>()?.GetInventory();
                    if (inventory != null && inventory.IsFull()) { destroyable.OnAction(); }
                }
            });
        }
    }
}