using System;
using System.Collections.Generic;
using System.Linq;

namespace WurstModReloaded
{
    internal static class AssemblyScanner
    {
        // This is set to true when a new assembly is loaded. It will let us know if our cache is potentially out of date.
        private static bool _invalid = false;

        // This is our cache of classes from the app domain of a given type.
        private static Dictionary<Type, object[]> _typeCache = new();

        // Static constructor
        static AssemblyScanner()
        {
            AppDomain.CurrentDomain.AssemblyLoad += (_, _) => _invalid = true;
        }

        public static IEnumerable<T> ScanClasses<T>()
        {
            // If the cache is valid and we have it cached, return it.
            if (!_invalid && _typeCache.TryGetValue(typeof(T), out var result))
                return result.Cast<T>();

            // Otherwise scan the assemblies again for all classes that are assignable from T
            var found = new List<T>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                found.AddRange(from type in assembly.GetTypes() where type.IsAssignableFrom(typeof(T)) select (T) Activator.CreateInstance(type));

            // Set the cache
            _typeCache[typeof(T)] = found.Cast<object>().ToArray();

            // Return
            return found;
        }
    }
}