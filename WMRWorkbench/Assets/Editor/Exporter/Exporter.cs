using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WurstMod.Exporter
{
    class Exporter
    {
        [MenuItem("DEBUG/Test Export Scene")]
        public static void Export()
        {
            ExportScene(SceneManager.GetActiveScene());
        }


        private static Scene scene;
        private static string filename;
        private static List<string> mangled;

        public static void ExportScene(Scene toExport)
        {
            try
            {
                scene = toExport;
                filename = Manglers.ManglerExtensions.GetProcessedFilename(scene.name);

                // Make sure all directories we need exist.
                SetupDirs();

                // Mangle the editor assembly and place it in the output folder.
                MangleEditorAssembly();

                // Mangle scripts written in editor so bundle sets them up to be loaded by the game.
                MangleScripts();

                // Clean out old files.
                PreClean();

                // Create the actual bundle file.
                ExportBundle();

                // Clean out files we don't care about.
                PostClean();

                // Return scripts written in editor to usable state.
                DemangleScripts();

                // Mangle the bundle so H3VR scripts load in-game.
                MangleBundle();
            }
            catch (Exception e)
            {
                // If something goes wrong, make sure scripts are not mangled!
                DemangleScripts();
                Debug.LogError("Failed to export scene");
                Debug.LogError(e);
            }
        }

        private static void SetupDirs()
        {
            if (!Directory.Exists(Constants.BundleOutputPath)) Directory.CreateDirectory(Constants.BundleOutputPath);
        }

        private static void MangleEditorAssembly()
        {
            Manglers.AssemblyMangler.MangleEditorAssembly(filename);
        }

        private static void MangleScripts()
        {
            mangled = Manglers.ScriptMangler.MangleEditorScripts(filename);
        }

        private static void DemangleScripts()
        {
            Manglers.ScriptMangler.DemangleEditorScripts(mangled);
        }

        private static void PreClean()
        {
            string fullPath = Constants.BundleOutputPath + filename + Constants.BundleExtension;

            if (File.Exists(fullPath)) File.Delete(fullPath);
            if (File.Exists(fullPath + ".manifest")) File.Delete(fullPath + ".manifest");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles")) File.Delete(Constants.BundleOutputPath + "AssetBundles");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles.manifest")) File.Delete(Constants.BundleOutputPath + "AssetBundles.manifest");
        }

        private static void PostClean()
        {
            string fullPath = Constants.BundleOutputPath + filename + Constants.BundleExtension;

            if (File.Exists(fullPath + ".manifest")) File.Delete(fullPath + ".manifest");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles")) File.Delete(Constants.BundleOutputPath + "AssetBundles");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles.manifest")) File.Delete(Constants.BundleOutputPath + "AssetBundles.manifest");
        }

        private static void ExportBundle()
        {
            // WARNING: Compression is a problem for BundleMangler. Do not change this until
            // there is a fix. Or just don't fix it, Deli mods are compressed anyway, right?
            var buildOptions = BuildAssetBundleOptions.UncompressedAssetBundle; 
            var build = default(AssetBundleBuild);
            build.assetBundleName = filename + Constants.BundleExtension;
            build.assetNames = new[] { scene.path };
            BuildPipeline.BuildAssetBundles(Constants.BundleOutputPath, new[] { build }, buildOptions, BuildTarget.StandaloneWindows64);
        }

        private static void MangleBundle()
        {
            Manglers.BundleMangler.MangleBundle(Constants.BundleOutputPath + filename + Constants.BundleExtension);
        }
    }
}
