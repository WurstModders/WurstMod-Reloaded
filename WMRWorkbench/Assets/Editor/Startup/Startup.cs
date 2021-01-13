using System.IO;
using UnityEditor;

namespace WurstMod.Startup
{
    public static class Startup
    {
        [MenuItem("DEBUG/Import H3VR Code")]
        public static void Import()
        {
            string path = FindManagedPath();
            bool success = MangleAndImport(path);
            if (success) SetupDefines();
        }

        private static string FindManagedPath()
        {
            string pathToExe = EditorUtility.OpenFilePanel("Locate h3vr.exe", "", "exe");
            if (string.IsNullOrEmpty(pathToExe)) return "";
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
            
        }
    }
}