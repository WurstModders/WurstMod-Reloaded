using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WurstModReloaded
{
    internal static class Utilities
    {
        /// <summary>
        /// Enumerates the types that match the provided condition.
        /// </summary>
        public static IEnumerable<Type> TypesWhere(Predicate<Type> condition)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(condition.Invoke);
        }

        /// <summary>
        /// Copies the fields of a monobehaviour to another game object, replacing the existing one if it already exists
        /// </summary>
        public static void CopyBehaviourTo<T>(T source, GameObject destObject, string[] ignoredFields = null) where T : MonoBehaviour
        {
            // Disable the game object so it won't fire any events
            destObject.SetActive(false);

            // Get a reference to the existing component
            var exist = destObject.GetComponent<T>();

            // Add a new one
            var dest = destObject.AddComponent<T>();

            // Iterate over each field in the source and copy it to the new one, using values from the old one where applicable
            ignoredFields ??= new string[0];
            foreach (var field in source.GetType().GetFields())
                field.SetValue(dest, !ignoredFields.Contains(field.Name) ? field.GetValue(source) : field.GetValue(exist));

            // Destroy the old object
            Object.Destroy(exist);
            
            // Reactivate the game object and let it Awake()
            destObject.SetActive(true);
        }
    }
}