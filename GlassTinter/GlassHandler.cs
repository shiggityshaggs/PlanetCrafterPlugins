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
        private ConfigEntry<int> red;
        private ConfigEntry<int> green;
        private ConfigEntry<int> blue;
        private ConfigEntry<int> alpha;

        private readonly ConfigDescription redDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private readonly ConfigDescription greenDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private readonly ConfigDescription blueDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private readonly ConfigDescription alphaDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 100));

        private readonly HashSet<Material> GlassMaterials = new();

        private void Start()
        {
            Initialize();
            PopulateHashSet();
            IterateGlass();
            StartCoroutine(ReloadConfig());
        }

        private void Initialize()
        {
            red = Config.Bind(section: "Glass", key: "Red", defaultValue: 0, configDescription: redDescription);
            green = Config.Bind(section: "Glass", key: "Green", defaultValue: 190, configDescription: greenDescription);
            blue = Config.Bind(section: "Glass", key: "Blue", defaultValue: 255, configDescription: blueDescription);
            alpha = Config.Bind(section: "Glass", key: "Alpha", defaultValue: 0, configDescription: alphaDescription);
            Config.SettingChanged += Config_SettingChanged;
        }

        private void PopulateHashSet()
        {
            Material[] materials = UnityEngine.Resources.FindObjectsOfTypeAll<Material>();
            foreach (Material material in materials)
            {
                if (material == null || (material.name != "Glass" && material.name != "GlassBiodome2")) continue;
                GlassMaterials.Add(material);
            }
        }

        private IEnumerator ReloadConfig()
        {
            while (true)
            {
                try
                {
                    Config.Reload();
                }
                catch
                {
                    // Sharing violation. No worries; retry in a second.
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void Config_SettingChanged(object sender, SettingChangedEventArgs e)
        {
            Debug.Log($"{DateTime.Now}\t{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}\tconfig changed");
            IterateGlass();
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