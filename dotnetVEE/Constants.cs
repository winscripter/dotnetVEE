namespace dotnetVEE
{
    /// <summary>
    /// Private string constants used by dotnetVEE.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Preferred FFmpeg index formatting.
        /// </summary>
        public const string PreferredIndexingName = "frame%07d.png";

        /// <summary>
        /// Similar to <see cref="PreferredIndexingName" />.
        /// </summary>
        public const int TrailingPadNumberCount = 7;
    }
}
