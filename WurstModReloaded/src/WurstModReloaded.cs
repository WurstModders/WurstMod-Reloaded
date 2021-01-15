﻿using System.Collections;
using System.Linq;
using BepInEx.Logging;
using Deli;
using UnityEngine;

namespace WurstModReloaded
{
    /// <summary>
    /// Main WurstMod Reloaded class. This is a monobehaviour that is applied to a global game object when the game starts.
    /// </summary>
    public class WurstModReloaded : DeliBehaviour
    {
        internal static WurstModReloaded Instance;

        internal new ManualLogSource Logger => base.Logger;

        private void Awake()
        {
            // Set a singleton instance
            Instance = this;

            // Register a callback when all Deli mods are done loading
            Deli.Deli.RuntimeComplete += DeliOnRuntimeComplete;

            // Init config
            WMRConfig.Init(Config);
            
            // Let everyone know we're working!
            Logger.LogInfo($"WurstMod Reloaded {Source.Info.Version} is awake! (H3VR Build ID: {Constants.BuildId})");
        }

        private void DeliOnRuntimeComplete()
        {
            // Some debug info
            Logger.LogInfo($"{DeliLoaderModule.Levels.Count} custom levels found!");

            // Output if the previous version of WurstMod is also loaded.
            if (Deli.Deli.Mods.Any(m => m.Info.Guid == "wurstmod")) Logger.LogWarning("The legacy version of WurstMod is installed. Please be aware this may cause issues.");
            
            // DEBUG
            StartCoroutine(DebugWaitForKey());
        }

        private IEnumerator DebugWaitForKey()
        {
            while (!Input.GetKeyDown(KeyCode.R))
                yield return null;
            StartCoroutine(Loader.LoadScene(DeliLoaderModule.Levels[0]));
        }
    }
}