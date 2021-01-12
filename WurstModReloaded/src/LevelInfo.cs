using Deli;
using UnityEngine;

namespace WurstModReloaded
{
    public struct LevelInfo
    {
        // JSON Serialized data
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int GameBuildId { get; set; }

        // Deli filled data
        public Mod Source { get; set; }
        public string AssetBundleLocation { get; set; }

        private AssetBundle _assetBundle;

        public AssetBundle AssetBundle
        {
            get
            {
                if (!_assetBundle)
                    _assetBundle = Source.Resources.Get<AssetBundle>(AssetBundleLocation).Expect("Could not find asset bundle for level");
                return _assetBundle;
            }
        }
    }
}