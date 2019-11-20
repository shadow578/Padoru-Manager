using System;
using System.Collections.Generic;

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
        /// Has this list any item that matches the given function
        /// </summary>
        /// <typeparam name="T">the list type</typeparam>
        /// <param name="list">the list to check</param>
        /// <param name="matchFunc">the functions at least one item has to match</param>
        /// <returns>does at least one item match the given function?</returns>
        public static bool HasAnyWhere<T>(this List<T> list, Func<T, bool> matchFunc)
        {
            foreach (T item in list)
            {
                if (matchFunc(item)) return true;
            }
            return false;
        }
    }
}
