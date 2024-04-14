using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.ImageUtils
{
    /// <summary>
    /// Adds glitch effect to an <b>image</b>.
    /// </summary>
    public class Glitch
    {
        /// <summary>
        /// Applies glitch effect to an image.
        /// </summary>
        /// <param name="inputPath">Input path to an image.</param>
        /// <param name="outputPath">Output path of an image.</param>
        /// <param name="probability">
        /// Chance to glitch a row.
        /// <br />
        /// <br />
        /// This value <i>must</i> range between 0 and 1. For instance, <c>0.2</c> would represent a
        /// 20% chance to glitch a row, which would likely provide more rows having a glitch effect applied.
        /// </param>
        /// <param name="shiftMin">Minimum shift amount</param>
        /// <param name="shiftMax">Maximum shift amount</param>
        public static void Apply(string inputPath, string outputPath, float probability = 0.1F, int shiftMin = -30, int shiftMax = 30)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(inputPath);
            var random = new Random();

            if (probability is < 0.0F or > 1.0F)
            {
                throw new ArgumentException("Value must range between 0 and 1", nameof(probability));
            }

            image.Mutate(ctx =>
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (random.NextDouble() < probability)
                    {
                        int shift = random.Next(shiftMin, shiftMax); // Shift amount

                        if (shift < 0)
                        {
                            for (int x = 0; x < image.Width + shift; x++)
                            {
                                image[x, y] = image[x - shift, y];
                            }
                        }
                        else
                        {
                            for (int x = image.Width - 1; x >= shift; x--)
                            {
                                image[x, y] = image[x - shift, y];
                            }
                        }
                    }
                }
            });

            image.Save(outputPath);
        }
    }
}
