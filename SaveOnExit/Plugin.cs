using BepInEx;
using HarmonyLib;
using SpaceCraft;

namespace SSPCP_SaveOnExit
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            Harmony harmony = new("SSPCP_SaveOnExit");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(UiWindowPause), "OnQuit")]
        private class SaveOnExit
        {
            private static void Prefix(UiWindowPause __instance)
            {
                __instance.OnSave();
            }
        }
    }
}