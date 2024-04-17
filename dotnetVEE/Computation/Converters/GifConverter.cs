using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;

namespace dotnetVEE.Computation.Converters
{
    /// <summary>
    /// Converts video files to GIF image format.
    /// </summary>
    public class GifConverter
    {
        /// <summary>
        /// Converts a video file to a GIF image format.
        /// </summary>
        /// <param name="videoPath">Path to the video file.</param>
        /// <returns>Path to the output GIF image file.</returns>
        /// <remarks>
        ///     <b>Warning</b>: Be aware of performance and the size of the GIF
        ///                     file. Larger videos can result in astonishingly
        ///                     large GIF image files. The quality of the output
        ///                     image may not be the best.<br />
        /// </remarks>
        public static string FromVideo(string videoPath)
        {
            string outputGif = RandomPathGenerator.GenerateRandomFileWithExtensionV1("gif");
            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -i \"{videoPath}\" \"{outputGif}\"");

            return outputGif;
        }

        /// <summary>
        /// Converts a video file to a GIF image format.
        /// </summary>
        /// <param name="video">Path &amp; information to/of the video file.</param>
        /// <returns>Path to the output GIF image file.</returns>
        /// <remarks>
        ///     <b>Warning</b>: Be aware of performance and the size of the GIF
        ///                     file. Larger videos can result in astonishingly
        ///                     large GIF image files. The quality of the output
        ///                     image may not be the best.<br />
        /// </remarks>
        public static string FromVideo(Video video) => FromVideo(video.Path);
    }
}
