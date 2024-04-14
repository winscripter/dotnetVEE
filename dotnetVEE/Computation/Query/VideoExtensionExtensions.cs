using dotnetVEE.Private.Extensions;

namespace dotnetVEE.Computation.Query
{
    /// <summary>
    /// Extensions for <see cref="VideoExtension" />.
    /// </summary>
    public static class VideoExtensionExtensions
    {
        /// <summary>
        /// Defines a file extension of a video.
        /// </summary>
        /// <param name="videoFile">Path to video.</param>
        /// <returns>Extension of this video.</returns>
        public static VideoExtension DefineExtension(this string videoFile)
        {
            Enum.TryParse(videoFile.Capitalize(), out VideoExtension videoExtension);
            return videoExtension;
        }
    }
}
