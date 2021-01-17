using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WurstMod.Manglers
{
    /// <summary>
    /// Scripts written in the Editor can be mangled to be loadable by the game by switching
    /// their MonoScript data to the final assembly that's generated alongside the bundle.
    /// The same cannot be done with H3's scripts, since they would need to be changed back to
    /// Assembly-CSharp BEFORE bundle creation, resulting in massive errors due to overlap.
    /// </summary>
    public static class ScriptMangler
    {
        /// <summary>
        /// Mangles scripts written in the Editor to be loadable after the bundle
        /// is exported, and provides a list of scripts that will need to be demangled
        /// after bundle export.
        /// </summary>
        public static List<string> MangleEditorScripts(string dllName)
        {
            List<string> toRestore = new List<string>();
            MethodInfo monoScriptInit = typeof(MonoScript).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (string path in AssetDatabase.GetAllAssetPaths().Where(x => x.EndsWith(".cs")))
            {
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script == null) continue;
                var @class = script.GetClass();
                if (@class == null || @class.Assembly.GetName().Name != Constants.NameMainAssembly) continue;
                toRestore.Add(path);

                // internal extern void Init(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript);
                monoScriptInit.Invoke(script, new object[] { script.text, script.name, script.GetClass().Namespace, dllName + ".dll", false });
            }

            return toRestore;
        }

        /// <summary>
        /// Reload mangled scripts to make them usable in the Editor again
        /// once the bundle export is complete.
        /// </summary>
        public static void DemangleEditorScripts(List<string> scripts)
        {
            foreach (string path in scripts)
            {
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script != null)
                {
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}