﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WurstMod.Manglers
{
    public class BundleMangler
    {
        /// <summary>
        /// Post-process an assetbundle to make it loadable by the game.
        /// </summary>
        /// <param name="name">Name for the level or asset</param>
        /// <param name="path">Path to the assetbundle</param>
        public static void MangleBundle(string name, string path)
        {
            byte[] nameMainAssembly = Encoding.ASCII.GetBytes(Constants.NameMainAssembly);
            byte[] nameFirstPass = Encoding.ASCII.GetBytes(Constants.NameFirstPass);
            byte[] renameMainAssembly = Encoding.ASCII.GetBytes(Constants.RenameMainAssembly);
            byte[] renameFirstPass = Encoding.ASCII.GetBytes(Constants.RenameFirstPass);

            byte[] bytes = File.ReadAllBytes(path);

            // NOTE the byte-swapping method I'm doing is technically unsafe, but the odds of an error happening are microscopic.
            // The "correct" way to do this is to read in the bytes of the bundle and follow the file structure to find the places that need to be changed.
            // I don't have time for that and I like those odds!

            // In the input file, Scripts defined by users are expected to exist in Assembly-CSharp.dll.
            // This is incorrect, since that would be loaded as H3VR's assembly.
            // Those scripts must be translated to an output name, which should be unique to the level they are generated by.
            // TODO This is misguided and DOES NOT WORK!
            string dllFilename = ManglerExtensions.GetProcessedFilename(name);
            byte[] dllFilenameBytes = Encoding.ASCII.GetBytes(dllFilename);
            List<int> matchIndices = bytes.FindAllSequenceMatchIndices(nameMainAssembly);
            foreach (int idx in matchIndices) Buffer.BlockCopy(dllFilenameBytes, 0, bytes, idx, dllFilenameBytes.Length);

            // Next, remap the mangled DLL references back to game DLL references.
            matchIndices = bytes.FindAllSequenceMatchIndices(renameMainAssembly);
            foreach (int idx in matchIndices) Buffer.BlockCopy(nameMainAssembly, 0, bytes, idx, nameMainAssembly.Length);
            matchIndices = bytes.FindAllSequenceMatchIndices(renameFirstPass);
            foreach (int idx in matchIndices) Buffer.BlockCopy(nameFirstPass, 0, bytes, idx, nameFirstPass.Length);

            // Finally, just write the file back.
            File.WriteAllBytes(path, bytes);
        }
    }
}