using BepInEx.Configuration;

namespace WurstModReloaded
{
    internal class Config
    {
        public static ConfigEntry<bool> LoadDebugLevels;

        public void Init(ConfigFile configFile)
        {
            LoadDebugLevels = configFile.Bind("Debug", "LoadDebugLevels", true, "True if yo u want the included default debug levels to be loaded");
        }
    }
}