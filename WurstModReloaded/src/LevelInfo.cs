using System.IO;
using Deli;
using UnityEngine;
using Valve.Newtonsoft.Json;

namespace WurstModReloaded
{
    [JsonObject(MemberSerialization.OptIn)]
    public struct LevelInfo
    {
        // JSON Serialized data
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public string Description { get; set; }
        [JsonProperty] public string Author { get; set; }
        [JsonProperty] public int GameBuildId { get; set; }

        // Deli filled data
        public Mod Source { get; set; }
        public string LevelPath { get; set; }

        private AssetBundle _assetBundle;
        private Texture2D _thumbnail;

        public AssetBundle AssetBundle
        {
            get
            {
                if (!_assetBundle)
                    _assetBundle = Source.Resources.Get<AssetBundle>(Path.Combine(LevelPath, Constants.AssetBundleFilename)).Expect("Could not find asset bundle for level");
                return _assetBundle;
            }
        }
        public Texture2D Thumbnail
        {
            get
            {
                if (!_thumbnail)
                    _thumbnail = Source.Resources.Get<Texture2D>(Path.Combine(LevelPath, Constants.ThumbnailFilename)).UnwrapOr(null);
                return _thumbnail;
            }
        }
    }
}