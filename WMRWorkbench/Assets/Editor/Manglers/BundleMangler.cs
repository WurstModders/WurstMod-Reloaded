using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WurstMod.Manglers
{
    public static class BundleMangler
    {
        /// <summary>
        /// Post-process an assetbundle to make it loadable by the game.
        /// </summary>
        /// <param name="path">Path to the assetbundle</param>
        public static void MangleBundle(string path)
        {
            byte[] nameMainAssembly = Encoding.ASCII.GetBytes(Constants.NameMainAssembly);
            byte[] nameFirstPass = Encoding.ASCII.GetBytes(Constants.NameFirstPass);
            byte[] renameMainAssembly = Encoding.ASCII.GetBytes(Constants.RenameMainAssembly);
            byte[] renameFirstPass = Encoding.ASCII.GetBytes(Constants.RenameFirstPass);

            byte[] bytes = File.ReadAllBytes(path);

            // NOTE the byte-swapping method I'm doing is technically unsafe, but the odds of an error happening are microscopic.
            // The "correct" way to do this is to read in the bytes of the bundle and follow the file structure to find the places that need to be changed.

            // Remap the mangled DLL references back to game DLL references.
            List<int> matchIndices = bytes.FindAllSequenceMatchIndices(renameMainAssembly);
            foreach (int idx in matchIndices) Buffer.BlockCopy(nameMainAssembly, 0, bytes, idx, nameMainAssembly.Length);
            matchIndices = bytes.FindAllSequenceMatchIndices(renameFirstPass);
            foreach (int idx in matchIndices) Buffer.BlockCopy(nameFirstPass, 0, bytes, idx, nameFirstPass.Length);

            // Finally, just write the file back.
            File.WriteAllBytes(path, bytes);
        }

        [MenuItem("WurstMod/Bundle Mangle Utility")]
        public static void MangleSelected()
        {
            var file = EditorUtility.OpenFilePanel("Select an asset bundle", string.Empty, string.Empty);
            MangleBundle(file);
            Debug.Log("Done!");
        }
    }
}