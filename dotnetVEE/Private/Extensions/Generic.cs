namespace dotnetVEE.Private.Extensions
{
    /// <summary>
    /// Generic extension methods.
    /// </summary>
    internal static class Generic
    {
        /// <summary>
        /// Copies a <see langword="struct" />.
        /// </summary>
        /// <typeparam name="T">Type of the struct.</typeparam>
        /// <param name="value">Struct to copy.</param>
        /// <returns>A copied memory-safe clone of the given struct.</returns>
        public static T CopyValueType<T>(this T value) where T : struct => value;
    }
}
