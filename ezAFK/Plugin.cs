using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using SpaceCraft;

namespace ezAFK
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private int originalTargetFramerate;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "ezAFK");
            Application.focusChanged += Application_focusChanged;
        }

        private void Application_focusChanged(bool hasFocus)
        {
            FPS();
            GaugeStatus();
        }

        private void FPS()
        {
            if (!Application.isFocused)
            {
                originalTargetFramerate = Application.targetFrameRate;
                Application.targetFrameRate = 5;
            }
            else
            {
                if (!originalTargetFramerate.Equals(default))
                Application.targetFrameRate = originalTargetFramerate;
            }
        }

        private void GaugeStatus()
        {
            PlayerGaugesHandler playerGaugesHandler = Managers.GetManager<PlayersManager>()?.GetActivePlayerController()?.GetPlayerGaugesHandler();
            playerGaugesHandler?.GetPlayerGaugeHealth()?.SetInfinityStatus(!Application.isFocused);
            playerGaugesHandler?.GetPlayerGaugeOxygen()?.SetInfinityStatus(!Application.isFocused);
            playerGaugesHandler?.GetPlayerGaugeThirst()?.SetInfinityStatus(!Application.isFocused);
        }
    }
}