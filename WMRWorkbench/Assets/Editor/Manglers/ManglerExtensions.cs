using System.Collections.Generic;
using System.Linq;

namespace WurstMod.Manglers
{
    public static class ManglerExtensions
    {
        /// <summary>
        /// Returns all start indices for instances of "toMatch" sequences inside of "data"
        /// </summary>
        public static List<int> FindAllSequenceMatchIndices<T>(this IEnumerable<T> data, IEnumerable<T> toMatch)
        {
            List<T> list = data.ToList();
            List<T> match = toMatch.ToList();
            List<int> ret = new List<int>();

            for (int ii = 0; ii < list.Count; ii++)
            {
                if (list[ii].Equals(match[0]))
                {
                    for (int jj = 0; jj < match.Count; jj++)
                    {
                        if (!list[ii + jj].Equals(match[jj])) break;
                        if (jj == match.Count - 1) ret.Add(ii);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Pads or truncates any string to the proper length for bundle hacking.
        /// This is meant to be used with the name of the level/asset being compiled.
        /// </summary>
        public static string GetProcessedFilename(string str)
        {
            // Either shorten or extend the filename so it is equal to the main assembly's length
            var processed = str.Length > Constants.NameMainAssembly.Length ? str.Substring(0, Constants.NameMainAssembly.Length) : str.PadRight(Constants.NameMainAssembly.Length, '_');
            return processed.ToLower();
        }
    }
}
