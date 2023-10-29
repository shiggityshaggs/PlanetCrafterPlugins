using HarmonyLib;
using SpaceCraft;
using UnityEngine;

namespace ezAFK
{
    internal class Patches
    {
        [HarmonyPatch(typeof(PlayerGaugeHealth), nameof(PlayerGaugeHealth.AddToCurrentValue))]
        class AFKHealth
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }

        [HarmonyPatch(typeof(PlayerGaugeThirst), nameof(PlayerGaugeThirst.AddToCurrentValue))]
        class AFKThirst
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }

        [HarmonyPatch(typeof(PlayerGaugeOxygen), nameof(PlayerGaugeOxygen.AddToCurrentValue))]
        class AFKOxygen
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }
    }
}