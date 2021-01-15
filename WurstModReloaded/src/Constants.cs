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

        // Reserved object names
        public const string CameraRigSpawn = "[CameraRig]";
    }
}