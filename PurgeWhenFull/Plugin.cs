using BepInEx;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SSPCP_PurgeWhenFull;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static List<string> incompatiblePlugins = new()
    {
        "CheatInventoryStacking"
    };

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        bool compatible = true;
        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        foreach (Assembly assembly in assemblies)
        {
            string assemblyName = assembly.GetName().Name;
            if (incompatiblePlugins.Contains(assemblyName)) {
                compatible = false;
                Debug.Log($"{MyPluginInfo.PLUGIN_NAME} is not compatible with {assemblyName}. Yielding.");
                return;
            }
        }

        if (compatible) { StartCoroutine(Classes.Purger.Coroutine()); }
    }
}