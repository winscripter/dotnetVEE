using dotnetVEE.Computation.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.Options
{
    /// <summary>
    /// Options for text computing. Used by the <see cref="AddText" /> utility, and also
    /// by some other options structs.
    /// </summary>
    public struct TextComputationOptions
    {
        /// <summary>
        /// Text to add to the image.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Foreground color of the font.
        /// </summary>
        public Rgba32 Brush { get; set; }

        /// <summary>
        /// X/Y point where the text is inserted.
        /// </summary>
        public Point AddAt { get; set; }

        /// <summary>
        /// Font of the text. This is primarily a TTF/TTC/WOFF file. Does not require installation.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextComputationOptions" /> struct.
        /// </summary>
        /// <param name="text">Text to add to the image.</param>
        /// <param name="foreground">Style of the font. <i>Only works with variable fonts!</i></param>
        /// <param name="addAt">X/Y point where the text is inserted.</param>
        /// <param name="font">Font of the text. This is primarily a TTF/TTC/WOFF file. Does not require installation.</param>
        public TextComputationOptions(string text, Rgba32 foreground, Point addAt, Font font)
        {
            Text = text;
            Brush = foreground;
            AddAt = addAt;
            Font = font;
        }

        /// <summary>
        /// Draws the text on the image based on computation options of this instance. Does not
        /// dispose <paramref name="image" />.
        /// </summary>
        /// <param name="image">Image to draw text on.</param>
        public void DrawOnImage(Image image)
        {
            string text = Text;
            Font font = Font;
            Rgba32 foreground = Brush;
            Point position = AddAt;

            image.Mutate(x => x
                .DrawText(text, font, foreground, position)
            );
        }
    }
}
