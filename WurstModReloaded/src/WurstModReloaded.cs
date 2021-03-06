using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using Deli;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WurstModReloaded
{
    /// <summary>
    /// Main WurstMod Reloaded class. This is a monobehaviour that is applied to a global game object when the game starts.
    /// </summary>
    public class WurstModReloaded : DeliBehaviour
    {
        internal static WurstModReloaded Instance;

        internal new ManualLogSource Logger => base.Logger;

        public Dictionary<string, GameObject> Prefabs = new();

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

            tag = "WM:R";
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
            StartCoroutine(LoadScene(DeliLoaderModule.Levels[0]));
        }

        private IEnumerator LoadScene(LevelInfo level)
        {
            // Load the base scene async
            yield return SceneManager.LoadSceneAsync("ProvingGround");
            var baseScene = SceneManager.GetSceneByName("ProvingGround");

            // Copy any important prefabs we need and clean the base scene
            var roots = baseScene.GetRootGameObjects();
            CopyPrefabs(roots);
            CleanBaseScene(roots);


            // Load the custom scene async
            var sceneName = Path.GetFileNameWithoutExtension(level.AssetBundle.GetAllScenePaths()[0]);
            yield return SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(sceneName), LoadSceneMode.Additive);
            var customScene = SceneManager.GetSceneByName(sceneName);

            // Fire the handoff message on the roots
            foreach (var root in customScene.GetRootGameObjects())
                root.SendMessage("PrefabsHandoff", Prefabs, SendMessageOptions.DontRequireReceiver);
            
            // Merge them.
            SceneManager.MergeScenes(customScene, baseScene);
        }

        private void CopyPrefabs(GameObject[] roots)
        {
            Prefabs.Clear();
            string[] prefabs = {"ItemSpawner"};

            foreach (var go in roots.SelectMany(x => x.GetComponentsInChildren<Transform>()).Select(x => x.gameObject))
            foreach (var prefab in prefabs)
                if (go.name.Contains(prefab) && !Prefabs.ContainsKey(prefab))
                {
                    var copy = Instantiate(go, null);
                    Prefabs.Add(prefab, copy);
                }
        }
        
        private void CleanBaseScene(GameObject[] roots)
        {
            foreach (var filter in new[]
            {
                "_Animator_Spawning_",
                "_Boards",
                "_Env",
                "AILadderTest1"
                //"__SpawnOnLoad"
            })
            foreach (var go in roots.SelectMany(x => x.GetComponentsInChildren<Transform>()))
                if (go.name.Contains(filter) && !Prefabs.ContainsValue(go.gameObject))
                    Destroy(go.gameObject);
        }
    }
}