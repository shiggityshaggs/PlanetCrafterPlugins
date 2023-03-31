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

        private ConfigDescription redDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private ConfigDescription greenDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private ConfigDescription blueDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 255));
        private ConfigDescription alphaDescription = new(description: string.Empty, acceptableValues: new AcceptableValueRange<int>(0, 100));

        HashSet<Material> GlassMaterials = new();

        private bool init;

        private void Start()
        {
            Config.SaveOnConfigSet = true;
            StartCoroutine(ReloadConfig());
        }

        private IEnumerator ReloadConfig()
        {
            while (true)
            {
                if (!init) Initialize();
                if (init) Config.Reload();
                yield return new WaitForSeconds(1f);
            }
        }

        private void Initialize()
        {
            init = true;
            PopulateHashSet();

            red = Config.Bind(section: "Glass", key: "Red", defaultValue: 0, configDescription: redDescription);
            green = Config.Bind(section: "Glass", key: "Green", defaultValue: 190, configDescription: greenDescription);
            blue = Config.Bind(section: "Glass", key: "Blue", defaultValue: 255, configDescription: blueDescription);
            alpha = Config.Bind(section: "Glass", key: "Alpha", defaultValue: 0, configDescription: alphaDescription);

            red.SettingChanged += SomethingHasChanged;
            green.SettingChanged += SomethingHasChanged;
            blue.SettingChanged += SomethingHasChanged;
            alpha.SettingChanged += SomethingHasChanged;
        }

        private void SomethingHasChanged(object sender, EventArgs e)
        {
            IterateGlass();
        }

        private void PopulateHashSet()
        {
            var materials = UnityEngine.Resources.FindObjectsOfTypeAll<Material>();
            foreach (var material in materials)
            {
                if (material == null || material.name.ToLower() != "glass") continue;
                GlassMaterials.Add(material);
            }
        }

        private const float k = 0.003921568627451f; // 1 divided by 255 == 0.003921568627451

        private void IterateGlass()
        {
            foreach (Material material in GlassMaterials)
            {
                if (material == null)
                {
                    GlassMaterials.Remove(material);
                    continue;
                }

                material.color = new Color(
                    Mathf.Clamp01(red.Value * k),
                    Mathf.Clamp01(green.Value * k),
                    Mathf.Clamp01(blue.Value * k),
                    Mathf.Clamp01(alpha.Value * 0.01f));
            }
        }
    }
}
