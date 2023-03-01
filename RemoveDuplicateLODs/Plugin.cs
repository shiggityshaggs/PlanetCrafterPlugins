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
                && Keyboard.current.lKey.wasPressedThisFrame)
            {
                RemoveDuplicateLODs();
            }
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
            Debug.Log($"\r\nChecked {LODCount} LODs across {LODGroups.Count()} LODGroups.\r\nRemoved {DuplicateCount} LODs.\r\nDestroyed {DestroyCount} empty LODGroups.");
        }


        [Obsolete("I wrote this one first, but it's too easy to lose track of which iterator goes where. Use RemoveDuplicateLODs() instead.")]
        private void RemoveDuplicateLODs_for()
        {
            LODGroup[] LODGroups = Resources.FindObjectsOfTypeAll<LODGroup>();
            int DuplicateCount = 0;
            int LODCount = 0;
            int DestroyCount = 0;

            for (int i = LODGroups.Count() - 1; i >= 0; i--)
            {
                LODGroup LODGroup = LODGroups[i];
                Transform root = LODGroup.transform.GetRoot();
                LOD[] LODs = LODGroup.GetLODs();

                LODCount += LODs.Count();

                for (int j = LODs.Count() - 1; j >= 0; j--)
                {
                    LOD LOD = LODs[j];
                    Transform crawler = LODGroup.transform;
                    while (crawler != root)
                    {
                        crawler = crawler.parent;

                        LODGroup crawlerLOD = crawler.GetComponent<LODGroup>();
                        if (crawlerLOD == null) continue;
                        LOD[] crawlerLODs = crawlerLOD.GetLODs();

                        for (int k = crawlerLODs.Count() - 1; k >= 0; k--)
                        {
                            bool hasDuplicates = crawlerLODs[k].renderers.Intersect(LODs[j].renderers).ToArray().Count() > 0;
                            if (hasDuplicates)
                            {
                                DuplicateCount++;
                                LOD[] newLODs = LODs.Where(l => !l.Equals(LOD)).ToArray();
                                if (newLODs.Length > 0)
                                {
                                    LODGroup.SetLODs(newLODs);
                                }
                                else
                                {
                                    Destroy(LODGroups[i]);
                                    DestroyCount++;
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log($"\r\nChecked {LODCount} LODs across {LODGroups.Count()} LODGroups.\r\nRemoved {DuplicateCount} LODs.\r\nDestroyed {DestroyCount} empty LODGroups.");
        }
    }
}