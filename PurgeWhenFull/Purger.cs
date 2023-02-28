using SpaceCraft;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SSPCP_PurgeWhenFull
{
    internal class Purger
    {
        internal static IEnumerator Coroutine()
        {
            Debug.Log(Application.dataPath);
            for (;;)
            {
                PurgeIfFull(Object.FindObjectsOfType<ActionRecycle>());
                PurgeIfFull(Object.FindObjectsOfType<ActionDestroyInventory>());
                yield return new WaitForSeconds(1f);
            }
        }

        internal static WorldObjectAssociated GetWOA<T>(T t) where T : MonoBehaviour
        {
            Transform transform = t.transform;

            while (transform != transform.GetRoot())
            {
                transform = transform.GetParent();
                WorldObjectAssociated WOA = transform.GetComponent<WorldObjectAssociated>();
                if (WOA != null) return WOA;
            }

            return null;
        }

        internal static void PurgeIfFull<T>(T[] t) where T : MonoBehaviour
        {
            foreach (T actionable in t.ToList())
            {
                WorldObjectAssociated WOA = GetWOA(actionable);
                if (WOA == null) continue;

                string group = WOA.GetWorldObject()?.GetGroup()?.id;
                if (!group.StartsWith("Destructor") && !group.StartsWith("RecyclingMachine")) continue;

                Inventory inv = WOA.transform.GetComponent<InventoryAssociated>()?.GetInventory();
                if (inv == null || inv.IsEmpty()) continue;

                if (!Plugin.CheatInventoryStacking && inv.IsFull())
                {
                    MakeItGoPoof();
                    continue;
                }

                if (Plugin.CheatInventoryStacking && inv.GetInsideWorldObjects().Count >= inv.GetSize())
                {
                    MakeItGoPoof();
                }

                void MakeItGoPoof()
                {
                    actionable.GetComponent<ActionRecycle>()?.OnAction();
                    actionable.GetComponent<ActionDestroyInventory>()?.OnAction();
                }
            }
        }
    }
}