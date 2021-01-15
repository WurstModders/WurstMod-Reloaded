using System;
using System.Collections.Generic;
using System.Linq;

namespace WurstModReloaded
{
    internal static class AssemblyScanner
    {
        /// <summary>
        /// Enumerates the types that match the provided condition.
        /// </summary>
        public static IEnumerable<Type> TypesWhere(Predicate<Type> condition)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(condition.Invoke);
        }
    }
}