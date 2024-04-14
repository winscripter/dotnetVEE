using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.Options
{
    /// <summary>
    /// Options for static rectangle computing. Used by the <see cref="RectangleUtility" /> utility.
    /// </summary>
    public struct StaticRectangleOptions : IOptionCanDraw
    {
        /// <summary>
        /// Start point of the rectangle.
        /// </summary>
        public Point Start { get; init; }

        /// <summary>
        /// End point of the rectangle.
        /// </summary>
        public Point End { get; init; }

        /// <summary>
        /// Position of the rectangle.
        /// </summary>
        public Point Position { get; init; }

        /// <summary>
        /// Color of the rectangle.
        /// </summary>
        public RgbaRF Background { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticRectangleOptions" /> struct.
        /// </summary>
        /// <param name="start">Start point of the rectangle.</param>
        /// <param name="end">End point of the rectangle.</param>
        /// <param name="position">Position of the rectangle.</param>
        /// <param name="background">Color of the rectangle.</param>
        public StaticRectangleOptions(Point start, Point end, Point position, RgbaRF background)
        {
            this.Start = start;
            this.End = end;
            this.Position = position;
            this.Background = background;
        }

        /// <inheritdoc />
        public void DrawOn(Image image)
        {
            Rgba32 color = new Rgba32(
                this.Background.R,
                this.Background.G,
                this.Background.B,
                this.Background.A
            );

            // Have to use Hungarian Notation here. I hope you
            // do not mind. If you do, sorry. :-)
            PointF pfStart = new PointF(this.Start.X, this.Start.Y);
            PointF pfEnd = new PointF(this.End.X, this.End.Y);
            // OK, not using Hungarian Notation anymore.

            RectangleF borders = new RectangleF(pfStart, new SizeF(pfEnd));
            image.Mutate(x => x.Draw(color, 1, borders));
        }
    }
}
