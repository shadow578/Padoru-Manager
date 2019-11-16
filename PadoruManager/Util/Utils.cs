using System;

namespace PadoruManager.Util
{
    public static class Utils
    {
        /// <summary>
        /// check if b is contained in a, ignoring case
        /// </summary>
        /// <param name="a">the string to check in</param>
        /// <param name="b">the string to check for</param>
        /// <returns>is b contained in a</returns>
        public static bool ContainsIgnoreCase(this string a, string b)
        {
            return a.ToUpper().Contains(b.ToUpper());
        }

        /// <summary>
        /// Create a Relative path from the given paths
        /// </summary>
        /// <param name="basePath">the base path the resulting path should be relative to</param>
        /// <param name="fullPath">the full (absolute) path to make relative</param>
        /// <returns>the relative path</returns>
        public static string MakeRelativePath(string basePath, string fullPath)
        {
            //get paths in upper case
            string basePathUc = basePath.ToUpper();
            string fullPathUc = fullPath.ToUpper();

            //check that full path begins with the base path
            if (!fullPathUc.StartsWith(basePathUc)) throw new InvalidOperationException("The full path must start with the base path!");

            //remove base path from full path
            string relPath = fullPath.Substring(basePath.Length).TrimStart('\\');
            return relPath;
        }
    }
}
