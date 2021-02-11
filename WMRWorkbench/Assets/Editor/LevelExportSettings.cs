using System;
using UnityEngine;

#if H3VR_DEFINED
using Valve.Newtonsoft.Json.Linq;
#endif
namespace WurstMod
{
    public class LevelExportSettings : ScriptableObject
    {
        [Header("General level settings")] [Tooltip("The name of your level.")]
        public string Name;

        [Tooltip("The description of your level")]
        public string Description;

        [Tooltip("The names of any and all authors of the level, separated by commas.")]
        public string Author;

        [Tooltip("This really only matters if you're making a Take and Hold level. Example options: 'takeandhold', 'sandbox', 'meatfortress'.")]
        public string Gamemode;

        [Tooltip("The image that will show up with your level")]
        public Texture2D Thumbnail;

        [Tooltip("This is the version number of your level. Increment it when you make a change.")]
        public int Revision;

        // TODO: Optionally auto-generate a Deli archive from the level
        [Header("Deli settings")] [Tooltip("Check this if you want the exporter to automatically pack your level into a Deli mod on it's own.")]
        public bool AutoPackDeliMod;

        [Tooltip("Set this to the source download page of your level (After uploading it to BoneTome, for instance)")]
        public string SourceUrl;

        [Tooltip("Set your Deli mod's version here. This should match the version used on the source website for update notifications. Needs to be in the format: 0.0.0.0")]
        public string ModVersion;

        public bool IsValid()
        {
            var valid = true;
            if (string.IsNullOrEmpty(Name))
            {
                Debug.LogError("Export Settings: Name field cannot be empty");
                valid = false;
            }

            if (string.IsNullOrEmpty(Author))
            {
                Debug.LogError("Export Settings: Author field cannot be empty");
                valid = false;
            }

            if (string.IsNullOrEmpty(Gamemode))
            {
                Debug.LogError("Export Settings: Gamemode field cannot be empty");
                valid = false;
            }

            if (Revision < 0)
            {
                Debug.LogError("Export Settings: Revision must be positive");
                valid = false;
            }

            // Deli stuff.
            if (AutoPackDeliMod)
            {
                try
                {
                    new Version(ModVersion);
                }
                catch (Exception e)
                {
                    Debug.LogError("Export Settings: Deli mod version is invalid.");
                    valid = false;
                }
            }

            return valid;
        }

        public string GetLevelInfoString(string sceneName, int buildId)
        {
#if H3VR_DEFINED
            var jObject = new JObject();
            jObject["Name"] = Name;
            jObject["Description"] = Description;
            jObject["Author"] = Author;
            jObject["Gamemode"] = Gamemode;
            jObject["LevelRevision"] = Revision;
            jObject["AssetBundlePath"] = sceneName + Constants.BundleExtension;
            jObject["ThumbnailPath"] = sceneName + ".png";
            jObject["GameBuildId"] = buildId;
            jObject["ExporterVersion"] = Constants.ExporterVersion;
            return jObject.ToString();
#else
            return "";
#endif
        }
    }
}