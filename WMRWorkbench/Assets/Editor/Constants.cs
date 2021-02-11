namespace WurstMod
{
    public static class Constants
    {
        public const string NameMainAssembly =   "Assembly-CSharp";
        public const string RenameMainAssembly = "H3VRCode-CSharp";
        public const string NameFirstPass =   "Assembly-CSharp-firstpass";
        public const string RenameFirstPass = "H3VRCode-CSharp-passfirst";

        // List of types to remove from the main assembly so that when we import the same thing later in the editor there are no conflicts
        public static readonly string[] StripTypes =
        {
            "MaterialMapChannelPackerDefinition",
            "Alloy.PackedMapDefinition"
        };
        
        public const string UnityCodePluginPath = "Assets/Plugins/UnityCode/";
        public const string EditorAssemblyPath = "Library/ScriptAssemblies/";
        public const string BundleOutputPath = "AssetBundles/";

        public const string BundleExtension = ".bundle";

        public const string H3VRManifestFilename = "appmanifest_450540.acf";
        public const string PathToManifest = "../../" + H3VRManifestFilename;
        public const string H3VRCachedLocationFile = "H3Location.txt";

        public const int ExporterVersion = 0;

    }
}
