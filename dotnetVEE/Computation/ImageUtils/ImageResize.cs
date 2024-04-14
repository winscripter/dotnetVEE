using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.ImageUtils
{
    /// <summary>
    /// Quick methods to resize an image.
    /// </summary>
    public class ImageResize
    {
        /// <summary>
        /// Halves image size.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="preserveAspectRatio">Should the aspect ratio be preserved?</param>
        public static void ResizeInHalf(Image image, bool preserveAspectRatio = false)
        {
            int width = image.Width / 2;
            int height = image.Height / 2;

            // If aspect ratio is preserved, just set the height to 0.
            // SixLabors.ImageSharp will calculate the height automatically.
            image.Mutate(x => x.Resize(width, preserveAspectRatio ? 0 : height));
        }

        /// <summary>
        /// Resizes image whilst preserving aspect ratio or not.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="preserveAspectRatio">Should the aspect ratio be preserved?</param>
        public static void ResizeImage(Image image, int width, int height, bool preserveAspectRatio = false)
        {
            // If aspect ratio is preserved, just set the height to 0.
            // SixLabors.ImageSharp will calculate the height automatically.
            image.Mutate(x => x.Resize(width, preserveAspectRatio ? 0 : height));
        }
    }
}
