using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FistVR;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace WurstModReloaded
{
    public static class Loader
    {
        public static IEnumerator LoadScene(LevelInfo level)
        {
            // Load the base scene async
            var baseOp = SceneManager.LoadSceneAsync("ProvingGround");
            while (!baseOp.isDone)
                yield return null;
            var baseScene = SceneManager.GetSceneByName("ProvingGround");
            
            // Load the custom scene async
            var name = Path.GetFileNameWithoutExtension(level.AssetBundle.GetAllScenePaths()[0]);
            var customOp = SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(name), LoadSceneMode.Additive);
            while (!customOp.isDone)
                yield return null;
            var customScene = SceneManager.GetSceneByName(name);
            
            // Merge them.
            SceneManager.MergeScenes(customScene, baseScene);
        }
    }
}