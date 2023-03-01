using BepInEx;
using System.Collections;
using UnityEngine;

namespace SSPCP_MaxLOD
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            StartCoroutine(Worker());
        }

        private IEnumerator Worker()
        {
            while (true)
            {
                LODGroup[] LODGroups = UnityEngine.Resources.FindObjectsOfTypeAll<LODGroup>();

                foreach (LODGroup LG in LODGroups)
                {
                    LG?.ForceLOD(0);
                }

                yield return new WaitForSeconds(5f);
            }
        }
    }
}