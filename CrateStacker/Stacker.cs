using HarmonyLib;
using SpaceCraft;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SSPCP_CrateStacker
{
    internal class Stacker : MonoBehaviour
    {
        private static bool isCrate = false;
        public static GameObject ghostTarget;

        [HarmonyPatch(typeof(BuildConstraint), "GetIsRespected")]
        private class Constraints : MonoBehaviour
        {
            private static bool Prefix(ref bool __result, ref BuildConstraint __instance)
            {
                if (Keyboard.current.rightShiftKey.isPressed)
                {
                    __result = true;
                    return false;
                }

                RaycastHit hit;
                Transform t = Camera.main.transform;

                if (Physics.Raycast(origin: t.position, direction: t.TransformDirection(Vector3.forward), hitInfo: out hit, Mathf.Infinity))
                {
                    if (hit.transform.name == "Container1(Clone)")
                    {
                        isCrate = true;
                        ghostTarget = hit.transform.gameObject;
                        __result = true;
                        return false;
                    }
                }

                isCrate = false;
                ghostTarget = null;

                return true;
            }
        }

        [HarmonyPatch(typeof(ConstructibleGhost), "Place")]
        private class GhostPlacement
        {
            private static void Postfix(ref GameObject __result)
            {
                if (__result != null && isCrate && ghostTarget != null)
                {
                    WorldObject wo = __result.GetComponentInParent<WorldObjectAssociated>().GetWorldObject();
                    if (wo == null) { return; }

                    Vector3 pos = ghostTarget.transform.position;
                    Vector3 newPos = new(pos.x, __result.transform.position.y, pos.z);
                    Quaternion newRot = ghostTarget.transform.rotation;

                    __result.transform.position = newPos;
                    __result.transform.rotation = newRot;
                    wo.SetPositionAndRotation(newPos, newRot);

                    isCrate = false;
                    ghostTarget = null;
                }
            }
        }
    }
}
