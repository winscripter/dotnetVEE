using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Options.Common;
using dotnetVEE.Computation.Utils;
using SixLabors.ImageSharp;

namespace dotnetVEE.Computation.Options
{
    /// <summary>
    /// Represents options for a moving rectangle. Used by the <see cref="RectangleMotionUtility" /> utility.
    /// </summary>
    public struct RectangleMotionOptions
    {
        /// <summary>
        /// Basic options for drawing the rectangle itself.
        /// </summary>
        public RectangleMotionObjectOptions ObjectOptions { get; set; }

        /// <summary>
        /// Options describing the movement of the rectangle.
        /// </summary>
        public BaseMotionOptions MotionOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleMotionOptions" /> struct.
        /// </summary>
        /// <param name="objectOptions">Options for the actual rectangle visibility.</param>
        /// <param name="baseOptions">Base options describing the rectangle movement.</param>
        public RectangleMotionOptions(
            RectangleMotionObjectOptions objectOptions,
            BaseMotionOptions baseOptions)
        {
            ObjectOptions = objectOptions;
            MotionOptions = baseOptions;
        }
    }
}
