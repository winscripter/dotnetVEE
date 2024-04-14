namespace dotnetVEE.Private.Utils
{
    /// <summary>
    /// A helper for generating random file/directory names safely.
    /// By safely means also check if the generated file/directory name
    /// exists or not, and if so, try to generate another name.
    /// </summary>
    internal static class RandomPathGenerator
    {
        /// <summary>
        /// Variant 1 of random directory name generation, in the following format:
        ///     <para>
        ///         <c>
        ///             dotnetVEE_utiltmp_{RANDOM_INT64}_{a OR b}
        ///         </c>
        ///     </para>
        /// </summary>
        /// <returns>A randomly generated directory name.</returns>
        public static string GenerateDirectoryNameV1()
        {
            string name;
            while (Directory.Exists(name = Generate()))
            {
                name = Generate();
            }

            return name;

            static string Generate()
                => $"dotnetVEE_utiltmp_{Random.Shared.NextInt64(1, long.MaxValue)}_{(Random.Shared.Next(1, 2) == 1 ? 'a' : 'b')}";
        }

        /// <summary>
        /// Variant 1 of random file name generation with a custom file <paramref name="extension" />, in the following format:
        ///     <para>
        ///         <c>
        ///             dotnetVEE_tf_{RANDOM_INT64}{RANDOM_INT64}_{a OR b}.{<paramref name="extension" />}
        ///         </c>
        ///     </para>
        /// </summary>
        /// <remarks>
        /// The <c>tf</c> in the filename is an abbreviation for <b>Temporary File</b>.
        /// </remarks>
        /// <param name="extension">The file extension of the randomly generated file.</param>
        /// <returns>A randomly generated file name.</returns>
        public static string GenerateRandomFileWithExtensionV1(string extension)
        {
            string name;
            while (File.Exists(name = Generate()))
            {
                name = Generate();
            }

            return name;

            string Generate()
                => $"dotnetVEE_tf_{Random.Shared.NextInt64(1, long.MaxValue)}{Random.Shared.NextInt64(1, long.MaxValue)}_{(Random.Shared.Next(1, 2) == 1 ? 'a' : 'b')}.{extension}";
        }
    }
}
