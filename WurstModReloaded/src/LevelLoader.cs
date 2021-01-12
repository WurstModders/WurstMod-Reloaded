using System.Collections.Generic;
using System.IO;
using ADepIn;
using Deli;
using Valve.Newtonsoft.Json;

namespace WurstModReloaded
{
    [QuickNamedBind("WMR.Level")]
    public class LevelLoader : IAssetLoader
    {
        internal static List<LevelInfo> Levels = new();
        
        public void LoadAsset(IServiceKernel kernel, Mod mod, string path)
        {
            // If the user has disabled loading the included levels, don't load it
            if (!Config.LoadDebugLevels.Value && mod.Info.Guid == "wurstmodreloaded") return;
            
            // Try and get the config from the path
            var loc = Path.Combine(path, Constants.LevelInfoFilename);
            if (!mod.Resources.Get<string>(loc).MatchSome(out var str))
            {
                mod.Logger.LogWarning($"Level {path} is missing level info file. Skipping...");
            }

            // Now we have the info file we can populate it with other values
            var info = JsonConvert.DeserializeObject<LevelInfo>(str);
            info.Source = mod;
            info.LevelPath = path;
            
            // Add it to our list
            Levels.Add(info);
        }
    }
}