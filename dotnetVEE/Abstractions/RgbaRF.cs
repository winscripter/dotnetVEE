using SixLabors.ImageSharp.PixelFormats;

namespace dotnetVEE.Abstractions
{
    /// <summary>
    /// Represents a simple RGBA Record of Floats (abbreviated to RgbaRF).
    /// </summary>
    /// <param name="R">The R field (Red).</param>
    /// <param name="G">The G field (Green).</param>
    /// <param name="B">The B field (Blue).</param>
    /// <param name="A">The A field (Alpha).</param>
    public record struct RgbaRF(
        float R,
        float G,
        float B,
        float A = 1F
    );

    /// <summary>
    /// Extensions for <see cref="RgbaRF" />.
    /// </summary>
    public static class RgbaRFExtensions
    {
        /// <summary>
        /// Converts the instance of <see cref="RgbaRF" /> to <see cref="Rgba32" />.
        /// </summary>
        /// <param name="rf">The instance of <see cref="RgbaRF" /> to convert.</param>
        /// <returns>An instance of the <see cref="Rgba32" /> struct from SixLabors.ImageSharp, converted from <see cref="RgbaRF" />.</returns>
        public static Rgba32 ToSixLaborsRgba32(this RgbaRF rf)
            => new Rgba32(rf.R, rf.G, rf.B, rf.A);
    }
}
