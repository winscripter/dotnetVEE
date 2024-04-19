namespace dotnetVEE.Computation
{
    /// <summary>
    /// Extensions for <see cref="ChromaSubsamplingScheme" />/
    /// </summary>
    public static class ChromaSubsamplingSchemeExtensions
    {
        /// <summary>
        /// Converts this scheme to a string.
        /// </summary>
        /// <param name="scheme">Scheme to stringify.</param>
        /// <returns>A string representation of a given chroma subsampling scheme.</returns>
        public static string StringifyScheme(this ChromaSubsamplingScheme scheme)
            => scheme.ToString().ToLowerInvariant();
    }
}
