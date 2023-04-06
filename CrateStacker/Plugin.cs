using BepInEx;
using HarmonyLib;

namespace SSPCP_CrateStacker
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();
            gameObject.AddComponent<Stacker>();
        }
    }
}