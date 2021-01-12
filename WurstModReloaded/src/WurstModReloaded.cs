using Deli;

namespace WurstModReloaded
{
    /// <summary>
    /// Main WurstMod Reloaded class. This is a monobehaviour that is applied to a global game object when the game starts.
    /// </summary>
    public class WurstModReloaded : DeliBehaviour
    {
        internal WurstModReloaded Instance;
        
        private void Awake()
        {
            // Set a singleton instance
            Instance = this;
            
            // Register a callback when all Deli mods are done loading
            Deli.Deli.RuntimeComplete += DeliOnRuntimeComplete;
            
            // Let everyone know we're working!
            Logger.LogInfo($"WurstMod Reloaded {Source.Info.Version} is awake! (H3VR Build ID: {Constants.BuildId})");
        }

        private static void DeliOnRuntimeComplete()
        {
            
        }
    }
}