using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Utils;
using SixLabors.ImageSharp;
using System.Runtime.CompilerServices;

namespace dotnetVEE.Computation.Options
{
    /// <summary>
    /// Represents information about a rectangle's visibility. This is similar
    /// to the <see cref="StaticRectangleOptions" /> struct, except this has less
    /// properties. Used by <see cref="RectangleMotionUtility" /> and <see cref="RectangleMotionOptions" />.
    /// </summary>
    public struct RectangleMotionObjectOptions
    {
        /// <summary>
        /// Size of the rectangle in pixels.
        /// </summary>
        public SizeF RectangleSize { get; init; }

        /// <summary>
        /// Thickness of the rectangle border in pixels.
        /// </summary>
        public int RectangleBorderThickness { get; init; }

        /// <summary>
        /// Color of the rectangle borders.
        /// </summary>
        public RgbaRF RectangleColor { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleMotionObjectOptions" /> struct.
        /// </summary>
        /// <param name="rectSize">Size of the rectangle in pixels.</param>
        /// <param name="rectBorderThickness">Thickness of the rectangle border in pixels.</param>
        /// <param name="rectColor">Color of the rectangle borders.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectangleMotionObjectOptions(
            SizeF rectSize,
            int rectBorderThickness,
            RgbaRF rectColor)
        {
            RectangleSize = rectSize;
            RectangleBorderThickness = rectBorderThickness;
            RectangleColor = rectColor;
        }
    }
}
