using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WurstMod.Exporter
{
    public static class BuildIDHelper
    {
        public static Regex buildIdMatch = new Regex(@"""buildid""\s+""(.*?)""");
        public static int? GetBuildID()
        {
            string fullPathToManifest = Startup.H3PathHelper.FindPath() + Constants.PathToManifest;
            if (File.Exists(fullPathToManifest))
            {
                string manifest = File.ReadAllText(fullPathToManifest);

                Match match = buildIdMatch.Match(manifest);
                if (match.Success)
                {
                    return int.Parse(match.Groups[1].ToString());
                }
            }

            Debug.LogError("Couldn't find BuildID! Is your H3VR installed outside of steam or pirated?");
            return null;
        }
    }
}