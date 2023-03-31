using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSPCP_GlassTinter
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    internal class GlassHandler : BaseUnityPlugin
    {
        private ConfigEntry<int> red, green, blue, alpha;

        private readonly ConfigDescription colorDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private readonly ConfigDescription alphaDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 100));

        private readonly HashSet<Material> GlassMaterials = new();

        private bool PendingChanges = true;

        private void Start()
        {
            Initialize();
            PopulateHashSet();
            StartCoroutine(ReloadConfig());
        }

        private void Initialize()
        {
            red = Config.Bind(section: "Glass", key: "Red", defaultValue: 0, configDescription: colorDescription);
            green = Config.Bind(section: "Glass", key: "Green", defaultValue: 190, configDescription: colorDescription);
            blue = Config.Bind(section: "Glass", key: "Blue", defaultValue: 255, configDescription: colorDescription);
            alpha = Config.Bind(section: "Glass", key: "Alpha", defaultValue: 0, configDescription: alphaDescription);

            Config.SettingChanged += Config_SettingChanged;
        }

        private void Config_SettingChanged(object sender, SettingChangedEventArgs e)
        {
            if (!PendingChanges)
            {
                PendingChanges = true;
                Debug.Log($"{DateTime.Now.ToString("HH:mm:ss")}\t{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION}\tconfig changed");
            }
        }

        private void PopulateHashSet()
        {
            Material[] materials = Resources.FindObjectsOfTypeAll<Material>();

            List<string> NamesToInclude = new()
            {
                "Glass",
                "GlassBiodome2"
            };

            foreach (Material material in materials)
            {
                if (material == null) continue;
                if (NamesToInclude.Contains(material.name)) GlassMaterials.Add(material);
            }
        }

        private IEnumerator ReloadConfig()
        {
            while (true)
            {
                try { Config.Reload(); } catch { /*Sharing violation. No worries; retry in a second.*/ } 

                if (PendingChanges)
                {
                    PendingChanges = false;
                    IterateGlass();
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private const float k = 0.003921568627451f; // 1 divided by 255

        private void IterateGlass()
        {
            foreach (Material material in GlassMaterials)
            {
                if (material == null) continue;

                material.color = new Color(
                    Mathf.Clamp01(red.Value * k),
                    Mathf.Clamp01(green.Value * k),
                    Mathf.Clamp01(blue.Value * k),
                    Mathf.Clamp01(alpha.Value * 0.01f));
            }
        }
    }
}