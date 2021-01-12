using ADepIn;
using Deli;

namespace WurstModReloaded
{
    [QuickNamedBind("WMR.Level")]
    public class LevelLoader : IAssetLoader
    {
        public void LoadAsset(IServiceKernel kernel, Mod mod, string path)
        {
            // If the user has disabled loading the included levels, don't load it
            if (!Config.LoadDebugLevels.Value && mod.Info.Guid == "wurstmodreloaded") return;
        }
    }
}