using SixLabors.ImageSharp;

namespace dotnetVEE.Computation.Converters
{
    /// <summary>
    /// Converts image formats with the help of <see cref="SixLabors.ImageSharp" />.
    /// </summary>
    public static class ImageConverter
    {
        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToBmp(this Image image, string output)
        {
            image.SaveAsBmp($"{output}.bmp");

            return Image.Load($"{output}.bmp");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToGif(this Image image, string output)
        {
            image.SaveAsGif($"{output}.gif");

            return Image.Load($"{output}.gif");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToJpeg(this Image image, string output)
        {
            image.SaveAsJpeg($"{output}.jpeg");

            return Image.Load($"{output}.jpeg");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToJpg(this Image image, string output)
        {
            image.SaveAsJpeg($"{output}.bmp");

            return Image.Load($"{output}.bmp");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToPbm(this Image image, string output)
        {
            image.SaveAsPbm($"{output}.pbm");

            return Image.Load($"{output}.pbm");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToPng(this Image image, string output)
        {
            image.SaveAsPng($"{output}.png");

            return Image.Load($"{output}.png");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToQoi(this Image image, string output)
        {
            image.SaveAsQoi($"{output}.qoi");

            return Image.Load($"{output}.qoi");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToTga(this Image image, string output)
        {
            image.SaveAsTga($"{output}.tga");

            return Image.Load($"{output}.tga");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToTiff(this Image image, string output)
        {
            image.SaveAsTiff($"{output}.tif");

            return Image.Load($"{output}.tif");
        }

        /// <summary>
        /// Uses <see cref="SixLabors.ImageSharp" /> to convert an image format
        /// to another. Supported image formats are the ones supported by
        /// <c>ImageSharp</c>, which at the time this was written, were:
        /// <list type="bullet">
        ///     <item>BMP</item>
        ///     <item>GIF</item>
        ///     <item>JPEG</item>
        ///     <item>PBM</item>
        ///     <item>PNG</item>
        ///     <item>QOI</item>
        ///     <item>TGA</item>
        ///     <item>TIFF</item>
        ///     <item>WEBP</item>
        /// </list>
        /// </summary>
        /// <param name="image">Input image format.</param>
        /// <param name="output">Output image file.</param>
        /// <returns>An <see cref="Image" /> instance pointing to the converted image file.</returns>
        public static Image ToWebp(this Image image, string output)
        {
            image.SaveAsWebp($"{output}.webp");

            return Image.Load($"{output}.webp");
        }
    }
}
