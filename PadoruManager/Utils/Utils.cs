namespace PadoruManager.Utils
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
    }
}
