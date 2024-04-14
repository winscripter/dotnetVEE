using SixLabors.ImageSharp;

namespace dotnetVEE.Abstractions
{
    /// <summary>
    /// Specifies an options struct that can draw on an image
    /// using its existing properties in its instance.
    /// </summary>
    public interface IOptionCanDraw
    {
        /// <summary>
        /// Draws the options struct on an image, with parameters being
        /// the properties with appropriate values in that instance.
        /// </summary>
        /// <param name="image">Image to draw on.</param>
        void DrawOn(Image image);
    }
}
