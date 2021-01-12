using Steamworks;

namespace WurstModReloaded
{
    internal static class Constants
    {
        static Constants()
        {
            // If the steam api is not initialized, initialize it.
            SteamAPI.Init();
            
            // Get the current build ID from steam
            BuildId = SteamApps.GetAppBuildId();
        }
        
        // Static readonly values
        public static readonly long BuildId;
        
        // Filenames
        public const string AssetBundleFilename = "leveldata";
        public const string LevelInfoFilename = "info.json";
        public const string ThumbnailFilename = "thumb.png";
    }
}