using BepInEx;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SSPCP_RemoveDuplicateLODs
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update()
        {
            // CTRL + L
            if (Keyboard.current.ctrlKey.isPressed
                && Keyboard.current.lKey.wasPressedThisFrame) { RemoveDuplicateLODs(); }
        }

        internal static void RemoveDuplicateLODs()
        {
            int DuplicateCount = 0;
            int LODCount = 0;
            int DestroyCount = 0;

            LODGroup[] LODGroups = Resources.FindObjectsOfTypeAll<LODGroup>();

            foreach (LODGroup LODGroup in LODGroups)
            {
                Transform root = LODGroup.transform.GetRoot();
                Transform crawler = LODGroup.transform;
                LOD[] LODs = LODGroup.GetLODs();
                LODCount += LODs.Count();

                foreach (LOD LOD in LODs)
                {
                    while (crawler != root)
                    {
                        crawler = crawler.parent;

                        LODGroup crawlerGroup = crawler.GetComponent<LODGroup>();
                        if (crawlerGroup == null) continue;

                        LOD[] crawlerLODs = crawlerGroup.GetLODs();
                        if (crawlerLODs.Count() == 0)
                        {
                            Destroy(crawlerGroup);
                            DestroyCount++;
                            continue;
                        }

                        foreach (LOD crawlerLOD in crawlerLODs)
                        {
                            bool isDuplicate = LOD.renderers.Intersect(crawlerLOD.renderers).ToArray().Count() > 0;
                            if (!isDuplicate) continue;
                            DuplicateCount++;

                            LOD[] newLODs = LODs.Where(L => !L.Equals(LOD)).ToArray();
                            if (newLODs.Count() > 0)
                            {
                                LODGroup.SetLODs(newLODs);
                            }
                            else
                            {
                                Destroy(LODGroup);
                                DestroyCount++;
                            }
                        }
                    }
                }
            }
            Debug.Log($"\r\n"
                + $"Checked {LODCount} LODs across {LODGroups.Count()} LODGroups.\r\n"
                + $"Removed {DuplicateCount} LODs.\r\n"
                + $"Destroyed {DestroyCount} empty LODGroups.");
        }
    }
}