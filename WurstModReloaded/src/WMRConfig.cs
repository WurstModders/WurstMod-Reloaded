using BepInEx.Configuration;

namespace WurstModReloaded
{
    internal static class WMRConfig
    {
        public static ConfigEntry<bool> LoadDebugLevels;

        public static void Init(ConfigFile configFile)
        {
            LoadDebugLevels = configFile.Bind("Debug", "LoadDebugLevels", true, "True if yo u want the included default debug levels to be loaded");
        }
    }
}