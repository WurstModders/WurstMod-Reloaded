using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WurstMod.Exporter
{
    static class Exporter
    {
        [MenuItem("WurstMod/Export Scene")]
        public static void Export()
        {
            // Force the user to save before continuing.
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            // Make sure the scene has an export settings object beside it
            var scene = SceneManager.GetActiveScene();
            var dir = Path.GetDirectoryName(scene.path) ?? "";
            var settingsLoc = Path.Combine(dir, scene.name + "_exportsettings.asset");
            var settings = AssetDatabase.LoadAssetAtPath<LevelExportSettings>(settingsLoc);
            
            // Get the build ID of H3. TODO: Cache this when the assemblies are imported
            var buildId = BuildIDHelper.GetBuildID();
            if (!buildId.HasValue)
            {
                EditorUtility.DisplayDialog("Export failed", "Could not determine the build id of H3VR", "Ok");
                return;
            }

            if (settings == null)
            {
                var empty = ScriptableObject.CreateInstance<LevelExportSettings>();
                AssetDatabase.CreateAsset(empty, settingsLoc);
                EditorUtility.DisplayDialog("Export failed", "You have not yet setup the scene's export settings. A file has been created for it beside your scene, please fill it out and then try again.", "Ok");
                return;
            }
            
            // Make sure the settings are valid
            if (!settings.IsValid())
            {
                EditorUtility.DisplayDialog("Export failed", "One or more fields on the export settings object are invalid. Check the debug console for more details.", "Ok");
                return;
            }
            
            // Call the export method
            var filename = ExportScene(scene);
            WriteExtras(settings, filename, buildId.Value);
        }

        public static string ExportScene(Scene toExport)
        {
            List<string> mangled = null;
            try
            {
                var filename = Manglers.ManglerExtensions.GetProcessedFilename(toExport.name);

                // Make sure all directories we need exist.
                if (!Directory.Exists(Constants.BundleOutputPath)) Directory.CreateDirectory(Constants.BundleOutputPath);

                // Mangle the editor assembly and place it in the output folder.
                Manglers.AssemblyMangler.MangleEditorAssembly(filename);

                // Mangle scripts written in editor so bundle sets them up to be loaded by the game.
                mangled = Manglers.ScriptMangler.MangleEditorScripts(filename);

                // Create the actual bundle file.
                ExportBundle(filename, toExport);

                // Clean out files we don't care about.
                PostClean(filename);

                // Mangle the bundle so H3VR scripts load in-game.
                Manglers.BundleMangler.MangleBundle(Constants.BundleOutputPath + filename + Constants.BundleExtension);

                // Log a message so the user knows we completed successfully.
                Debug.Log("Export finished! Asset bundle location: " + Constants.BundleOutputPath + filename + Constants.BundleExtension);

                return filename;
            }
            catch (Exception e)
            {
                Debug.LogError(new Exception("Failed to export scene", e));
            }
            // If the try finishes or we caught an error, return scripts to the state used in the editor
            finally
            {
                if (mangled != null) Manglers.ScriptMangler.DemangleEditorScripts(mangled);
            }

            return null;
        }

        private static void PostClean(string filename)
        {
            string fullPath = Constants.BundleOutputPath + filename + Constants.BundleExtension;

            if (File.Exists(fullPath + ".manifest")) File.Delete(fullPath + ".manifest");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles")) File.Delete(Constants.BundleOutputPath + "AssetBundles");
            if (File.Exists(Constants.BundleOutputPath + "AssetBundles.manifest")) File.Delete(Constants.BundleOutputPath + "AssetBundles.manifest");
        }

        private static void ExportBundle(string filename, Scene scene)
        {
            // WARNING: Compression is a problem for BundleMangler. Do not change this until
            // there is a fix. Or just don't fix it, Deli mods are compressed anyway, right?
            var buildOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
            var build = default(AssetBundleBuild);
            build.assetBundleName = filename + Constants.BundleExtension;
            build.assetNames = new[] {scene.path};
            BuildPipeline.BuildAssetBundles(Constants.BundleOutputPath, new[] {build}, buildOptions, BuildTarget.StandaloneWindows64);
        }

        private static void WriteExtras(LevelExportSettings settings, string filename, int buildId)
        {
            File.WriteAllText(Path.Combine(Constants.BundleOutputPath, "info.json"), settings.GetLevelInfoString(filename, buildId));
            if (settings.Thumbnail) File.WriteAllBytes(Path.Combine(Constants.BundleOutputPath, filename + ".png"), settings.Thumbnail.EncodeToPNG());
        }
    }
}