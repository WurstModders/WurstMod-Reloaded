using System.Collections.Generic;
using System.IO;
using ADepIn;
using Deli;
using FMOD;
using Valve.Newtonsoft.Json;

namespace WurstModReloaded
{
    [QuickNamedBind("WMR.Level")]
    public class DeliLoaderModule : IAssetLoader
    {
        internal static readonly List<LevelInfo> Levels = new();
        
        public void LoadAsset(IServiceKernel kernel, Mod mod, string path)
        {
            // If the user has disabled loading the included levels, don't load it
            if (!WMRConfig.LoadDebugLevels.Value && mod.Info.Guid == "wurstmodreloaded") return;
            
            // Try and get the config from the path
            if (!mod.Resources.Get<string>(path).MatchSome(out var str))
            {
                WurstModReloaded.Instance.Logger.LogWarning($"Level {mod.Info.Guid}:{path} is missing level info file. Skipping...");
            }

            // Now we have the info file we can populate it with other values
            WurstModReloaded.Instance.Logger.LogDebug(str);
            var info = JsonConvert.DeserializeObject<LevelInfo>(str);
            info.Source = mod;
            info.LevelPath = Path.GetDirectoryName(path);
            
            // If the level was created with a newer version of the game, output a warning and skip
            if (info.GameBuildId > Constants.BuildId)
            {
                WurstModReloaded.Instance.Logger.LogWarning($"Level {mod.Info.Guid}:{path} was built against a newer version of the game. Skipping...");
                return;
            }
            
            // Add it to our list
            Levels.Add(info);
            WurstModReloaded.Instance.Logger.LogDebug($"Level {mod.Info.Guid}:{path} was loaded!");
        }
    }
}