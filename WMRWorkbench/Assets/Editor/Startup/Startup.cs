using System.IO;
using UnityEngine;
using UnityEditor;

namespace WurstMod.Startup
{
    public static class Startup
    {
        [MenuItem("WurstMod/Import H3VR Code")]
        public static void Import()
        {
            string path = FindManagedPath();
            bool success = MangleAndImport(path);
            if (success) SetupDefines();
            else Debug.LogError("H3 Import failed!");
        }

        private static string FindManagedPath()
        {
            string directory = H3PathHelper.FindPath();
            string pathToExe = Path.Combine(directory, "h3vr.exe");
            if (!File.Exists(pathToExe))
            {
                pathToExe = EditorUtility.OpenFilePanel("Locate h3vr.exe", directory, "exe");
            }
           
            H3PathHelper.WriteCache(Path.GetDirectoryName(pathToExe) + '\\');
            return Path.GetDirectoryName(pathToExe) + "/h3vr_Data/Managed/";
        }

        private static bool MangleAndImport(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            Manglers.AssemblyMangler.MangleBuiltAssemblies(path);
            return true;
        }

        private static void SetupDefines()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "H3VR_IMPORTED");
        }
    }
}
