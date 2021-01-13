using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace WurstMod.Exporter
{
    class Exporter
    {
        [MenuItem("DEBUG/Test Export Scene")]
        public static void TestExport()
        {
            Scene toExport = SceneManager.GetActiveScene();
            Manglers.AssemblyMangler.MangleEditorAssembly(toExport.name);
            ExportScene(toExport);
        }

        public static void ExportScene(Scene scene)
        {
            var buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            var build = default(AssetBundleBuild);
            build.assetBundleName = Manglers.ManglerExtensions.GetProcessedFilename(scene.name) + Constants.BundleExtension;
            build.assetNames = new[] { scene.path };

            string fullPath = Constants.BundleOutputPath + build.assetBundleName;
            if (!Directory.Exists(Constants.BundleOutputPath)) Directory.CreateDirectory(Constants.BundleOutputPath);

            PreClean(fullPath);
            BuildPipeline.BuildAssetBundles(Constants.BundleOutputPath, new[] { build }, buildOptions, BuildTarget.StandaloneWindows64);
            PostClean(fullPath);

            Manglers.BundleMangler.MangleBundle(scene.name, fullPath);
        }

        private static void PreClean(string fullPath)
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            if (File.Exists(fullPath + ".manifest")) File.Delete(fullPath + ".manifest");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles")) File.Delete(Constants.BundleOutputPath + "AssetBundles");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles.manifest")) File.Delete(Constants.BundleOutputPath + "AssetBundles.manifest");
        }

        private static void PostClean(string fullPath)
        {
            if (File.Exists(fullPath + ".manifest")) File.Delete(fullPath + ".manifest");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles")) File.Delete(Constants.BundleOutputPath + "AssetBundles");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles.manifest")) File.Delete(Constants.BundleOutputPath + "AssetBundles.manifest");
        }
    }
}
