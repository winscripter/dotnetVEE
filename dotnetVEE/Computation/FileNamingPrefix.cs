namespace dotnetVEE.Computation
{
    /// <summary>
    /// Represents a file naming prefix - that is, a simplified ffmpeg
    /// output filename format specifically used in cases where a utility
    /// could result in multiple files. A great example is <c>frame_%06d.png</c>,
    /// which for a user who may not know FFmpeg index formatting, may simplify
    /// this formatting.
    /// </summary>
    /// <param name="Prefix">The prefix of the file appearing before the digit count.</param>
    /// <param name="DigitCount">The ascending digit count used to index output file names.</param>
    public record struct FileNamingPrefix(
        string Prefix,
        int DigitCount);

    /// <summary>
    /// Extensions for <see cref="FileNamingPrefix" />.
    /// </summary>
    public static class FileNamingPrefixExtensions
    {
        /// <summary>
        /// Represents <see cref="FileNamingPrefix" /> as a string, ready to be read by FFmpeg.
        /// </summary>
        /// <param name="prefix">The instance of <see cref="FileNamingPrefix" /> to convert to a string.</param>
        /// <returns>String representation of the given <see cref="FileNamingPrefix" /> that can be read by FFmpeg.</returns>
        public static string Stringify(this FileNamingPrefix prefix)
            => $"{prefix.Prefix}{new string('0', prefix.DigitCount)}";
    }
}
