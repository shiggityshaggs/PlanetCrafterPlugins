using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ezAFK
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "ezAFK");
            Application.focusChanged += FPS;
        }

        private int originalTargetFramerate;

        private void FPS(bool hasFocus)
        {
            if (!hasFocus)
            {
                originalTargetFramerate = Application.targetFrameRate;
                Application.targetFrameRate = 5;
            }
            else
            {
                Application.targetFrameRate = originalTargetFramerate;
            }
        }
    }
}