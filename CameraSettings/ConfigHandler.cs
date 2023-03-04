using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections;
using UnityEngine;

namespace SSPCP_CameraSettings
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    internal class ConfigHandler : BaseUnityPlugin
    {
        private ConfigEntry<int> targetFrameRate;
        private ConfigEntry<bool> allowHDR;
        private ConfigEntry<bool> allowMSAA;
        private ConfigEntry<float> fieldOfView;
        private ConfigEntry<float> farClipPlane;
        private ConfigEntry<bool> useOcclusionCulling;
        private ConfigEntry<bool> layerCullSpherical;
        private ConfigEntry<int> maximumLODLevel;
        private ConfigEntry<int> vSyncCount;

        private static AcceptableValueList<bool> TrueFalse = new(true, false);

        private ConfigDescription targetFrameRateDescription = new(description: "FPS (-1 is uncapped)", acceptableValues: new AcceptableValueRange<int>(-1, 240));
        private ConfigDescription allowHDRDescription = new(description: "High dynamic range", acceptableValues: TrueFalse);
        private ConfigDescription allowMSAADescription = new(description: "Multisample Anti-Aliasing", acceptableValues: TrueFalse);
        private ConfigDescription fieldOfViewDescription = new(description: "Field of View", acceptableValues: new AcceptableValueRange<float>(50, 179));
        private ConfigDescription farClipPlaneDescription = new(description: "View distance", acceptableValues: new AcceptableValueRange<float>(1000, 15000));
        private ConfigDescription useOcclusionCullingDescription = new(description: "Occlusion Culling", acceptableValues: TrueFalse);
        private ConfigDescription layerCullSphericalDescription = new(description: "View distance is spherical", acceptableValues: TrueFalse);
        private ConfigDescription maximumLODLevelDescription = new(description: "Max LOD level", new AcceptableValueRange<int>(0, 5));
        private ConfigDescription vSyncCountDescription = new(description: "Prevent screen tearing", new AcceptableValueRange<int>(0, 4));

        private bool init = false;

        private void Start()
        {
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

        void OnApplicationQuit()
        {
            Config.Save();
        }

        private void Initialize()
        {
            if (Camera.main == null) return;
            init = true;

            targetFrameRate = Config.Bind(section: "Application", key: "targetFrameRate", defaultValue: Application.targetFrameRate, targetFrameRateDescription);
            allowHDR = Config.Bind(section: "Camera", key: "allowHDR", defaultValue: true, allowHDRDescription);
            allowMSAA = Config.Bind(section: "Camera", key: "allowMSAA", defaultValue: true, allowMSAADescription);
            fieldOfView = Config.Bind(section: "Camera", key: "fieldOfView", defaultValue: Camera.main.fieldOfView, fieldOfViewDescription);
            farClipPlane = Config.Bind(section: "Camera", key: "farClipPlane", defaultValue: Camera.main.farClipPlane, farClipPlaneDescription);
            useOcclusionCulling = Config.Bind(section: "Camera", key: "useOcclusionCulling", defaultValue: true, useOcclusionCullingDescription);
            layerCullSpherical = Config.Bind(section: "Camera", key: "layerCullSpherical", defaultValue: Camera.main.layerCullSpherical, layerCullSphericalDescription);
            maximumLODLevel = Config.Bind(section: "QualitySettings", key: "maximumLODLevel", defaultValue: 0, maximumLODLevelDescription);
            vSyncCount = Config.Bind(section: "QualitySettings", key: "vSyncCount", defaultValue: 0, vSyncCountDescription);

            targetFrameRate.SettingChanged += (object sender, EventArgs e) => Application.targetFrameRate = targetFrameRate.Value;
            allowHDR.SettingChanged += (object sender, EventArgs e) => Camera.main.allowHDR = allowHDR.Value;
            allowMSAA.SettingChanged += (object sender, EventArgs e) => Camera.main.allowMSAA = allowMSAA.Value;
            fieldOfView.SettingChanged += (object sender, EventArgs e) => Camera.main.fieldOfView = fieldOfView.Value;
            farClipPlane.SettingChanged += (object sender, EventArgs e) => Camera.main.farClipPlane = farClipPlane.Value;
            useOcclusionCulling.SettingChanged += (object sender, EventArgs e) => Camera.main.useOcclusionCulling = useOcclusionCulling.Value;
            layerCullSpherical.SettingChanged += (object sender, EventArgs e) => Camera.main.layerCullSpherical = layerCullSpherical.Value;
            maximumLODLevel.SettingChanged += (object sender, EventArgs e) => QualitySettings.maximumLODLevel = maximumLODLevel.Value;
            vSyncCount.SettingChanged += (object sender, EventArgs e) => QualitySettings.vSyncCount = vSyncCount.Value;
        }
    }
}