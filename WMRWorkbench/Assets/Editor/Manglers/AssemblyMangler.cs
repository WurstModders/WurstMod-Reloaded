using System;
using System.IO;
using UnityEditor;
using Mono.Cecil;

namespace WurstMod.Manglers
{
    public static class AssemblyMangler
    {
        [MenuItem("DEBUG/Test Assembly Mangle + Import")]
        public static void TestMangle()
        {
            string pathToExe = EditorUtility.OpenFilePanel("Locate h3vr.exe", "", "exe");
            if (string.IsNullOrEmpty(pathToExe)) return;

            string path = Path.GetDirectoryName(pathToExe) + "/h3vr_Data/Managed/";
            MangleBuiltAssemblies(path);
            //MangleUnityAssemblies("F:/Games/Steam/steamapps/common/H3VR/h3vr_Data/Managed/");
        }


        /// <summary>
        /// Hack a built Unity assembly to allow it to be loaded directly into the Editor as a plugin.
        /// </summary>
        public static void MangleBuiltAssemblies(string pathToManaged)
        {
            // We must use a custom assembly resolver to successfully process the DLLs.
            ReaderParameters readerParams = new ReaderParameters
            {
                AssemblyResolver = new UnityAssemblyResolver(pathToManaged)
            };
            if (!Directory.Exists(Constants.UnityCodePluginPath)) Directory.CreateDirectory(Constants.UnityCodePluginPath);

            // PART 1: firstpass
            // This must be done first because the main assembly references it.
            // The filenames must be the same length.
            // The suffix "-firstpass" is reserved by Unity.
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly(pathToManaged + Constants.NameFirstPass + ".dll", readerParams);
            asm.Name = new AssemblyNameDefinition(Constants.RenameFirstPass, new Version(1, 0, 0, 0));
            asm.MainModule.Name = Constants.RenameFirstPass + ".dll";
            asm.Write(pathToManaged + Constants.RenameFirstPass + ".dll");
            asm.Write(Constants.UnityCodePluginPath + Constants.RenameFirstPass + ".dll");


            // PART 2: Main assembly
            // Similar constraints as above.
            // We also need to change references, since nothing is allowed to point at firstpass.
            asm = AssemblyDefinition.ReadAssembly(pathToManaged + Constants.NameMainAssembly + ".dll", readerParams);
            asm.Name = new AssemblyNameDefinition(Constants.RenameMainAssembly, new Version(1, 0, 0, 0));
            asm.MainModule.Name = Constants.RenameMainAssembly + ".dll";
            foreach (var ii in asm.MainModule.AssemblyReferences)
            {
                if (ii.Name == Constants.NameFirstPass) ii.Name = Constants.RenameFirstPass;
            }
            asm.Write(Constants.UnityCodePluginPath + Constants.RenameMainAssembly + ".dll");
            File.Delete(pathToManaged + Constants.RenameFirstPass + ".dll");


            // PART 3: References
            // The above assemblies may access libraries Unity doesn't have direct access to.
            // There's probably a good way to figure out which DLLs these are automatically, but
            // for now we will just copy the ones we know we need. TODO I guess.
            string[] files = { "DinoFracture.dll", "ES2.dll", "Valve.Newtonsoft.Json.dll" };
            foreach (string file in files) File.Copy(pathToManaged + file, Constants.UnityCodePluginPath + file, true);


            // Finally, kick the asset database to tell it to process the new files.
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Hack the Editor assembly located in Library/ScriptAssemblies to be loadable by the game.
        /// This method will place the modified dll into the AssetBundles folder.
        /// </summary>
        /// <param name="filename">Properly-formatted name (usually obtained via ManglerExtensions.GetProcessedFilename())</param>
        public static void MangleEditorAssembly(string filename)
        {
            if (!File.Exists(Constants.EditorAssemblyPath + Constants.NameMainAssembly + ".dll")) return;

            // Module name needs to be changed away from Assembly-CSharp.dll because it is a reserved name.
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly(Constants.EditorAssemblyPath + Constants.NameMainAssembly + ".dll");
            asm.Name = new AssemblyNameDefinition(filename, asm.Name.Version);
            asm.MainModule.Name = filename + ".dll";

            // References to renamed unity code must be swapped out.
            foreach(var ii in asm.MainModule.AssemblyReferences)
            {
                if (ii.Name == Constants.RenameMainAssembly) ii.Name = Constants.NameMainAssembly;
                if (ii.Name == Constants.RenameFirstPass) ii.Name = Constants.NameFirstPass;
            }

            // Save it into /AssetBundles.
            asm.Write(Constants.BundleOutputPath + filename + ".dll");
        }
    }
}