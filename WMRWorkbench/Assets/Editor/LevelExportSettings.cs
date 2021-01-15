using UnityEngine;

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

        [Tooltip("This is the version number of your level. Increment it when you make a change.")]
        public int Revision;
        
        // TODO: Optionally auto-generate a Deli archive from the level
        [Header("Deli settings")] [Tooltip("Check this if you want the exporter to automatically pack your level into a Deli mod on it's own.")] 
        public bool AutoPackDeliMod;

        [Tooltip("Set this to the source download page of your level (After uploading it to BoneTome, for instance)")]
        public string SourceUrl;
    }
}