using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;

namespace WurstMod.Startup
{
    public static class H3PathHelper
    {
        /// <summary>
        /// Attempts to find the path to the main H3VR directory using the registry.
        /// </summary>
        /// <returns></returns>
        public static string FindPath()
        {
            // Check cache.
            if (File.Exists(Constants.H3VRCachedLocationFile))
            {
                return File.ReadAllText(Constants.H3VRCachedLocationFile);
            }

            // Get the main steam installation location via registry.
            string steamDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", "") as string;
            if (steamDir == "") steamDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", "") as string;
            if (steamDir == "") return "";

            // Check main steamapps library folder for h3 manifest.
            string result = "";
            if (File.Exists(steamDir + @"\steamapps\" + Constants.H3VRManifestFilename))
            {
                result = steamDir + @"\steamapps\common\H3VR\";
            }
            else
            {
                // We didn't find it, look at other library folders by lazily parsing libraryfolders.
                List<string> lines = File.ReadAllLines(steamDir + "/steamapps/libraryfolders.vdf").Skip(4).Where(x => x.Length != 0 && x[0] != '}').ToList();
                lines = lines.Select(x => x.Split('\t')[3].Trim('"').Replace(@"\\", @"\")).ToList();
                foreach (string ii in lines)
                {
                    if (File.Exists(ii + @"\steamapps\" + Constants.H3VRManifestFilename))
                    {
                        result = ii + @"\steamapps\common\H3VR\";
                        break;
                    }
                }
            }

            if (result != "")
            {
                WriteCache(result);
            }

            return result;
        }

        public static void WriteCache(string path)
        {
            File.WriteAllText(Constants.H3VRCachedLocationFile, path);
        }
    }
}