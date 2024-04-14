using SixLabors.Fonts;

namespace dotnetVEE.Abstractions
{
    /// <summary>
    /// Represents information about a font.
    /// </summary>
    public struct FontInfo
    {
        /// <summary>
        /// Path to the font.
        /// </summary>
        public string FontPath { get; set; }

        /// <summary>
        /// The actual font.
        /// </summary>
        public Font Font { get; set; }
    }
}
