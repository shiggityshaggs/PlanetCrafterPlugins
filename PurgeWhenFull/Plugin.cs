using BepInEx;
using System;
using System.Linq;

namespace SSPCP_PurgeWhenFull;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static bool CheatInventoryStacking = false;

    private void Awake()
    {
        // Plugin startup logic
        CheatInventoryStacking = AppDomain.CurrentDomain.GetAssemblies().Count(assembly => assembly.GetName().Name == "CheatInventoryStacking") > 0;
        string info = $"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!";
        if (CheatInventoryStacking) info += " CheatInventoryStacking found.";
        Logger.LogInfo(info);

        StartCoroutine(Purger.Coroutine());
    }
}