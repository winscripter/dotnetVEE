using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation.ImageUtils;
using SixLabors.ImageSharp;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects.Extended
{
    /// <summary>
    /// Options for scaling into the image.
    /// </summary>
    /// <param name="X">X Scaling (Width)</param>
    /// <param name="Y">Y Scaling (Width)</param>
    public record ScaleOptions(
        int X, int Y);

    /// <summary>
    /// An effect to scale into the image.
    /// </summary>
    public static class Scale
    {
        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Options for zooming in/out.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, ScaleOptions options)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(Color.Black).ZoomImage(ref e, options.X, options.Y);
            });

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Options for zooming in/out.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, ScaleOptions options, DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(Color.Black).ZoomImage(ref e, options.X, options.Y);
            }, cleanupMode);

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="padColor">Background color that will be used when the frame is out of bounds.</param>
        /// <param name="options">Options for zooming in/out.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, Color padColor, ScaleOptions options)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(padColor).ZoomImage(ref e, options.X, options.Y);
            });

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="scalePx">Pixels to zoom in/out (applies both to X and Y).</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, int scalePx)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(Color.Black).ZoomImage(ref e, scalePx, scalePx);
            });

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Options for zooming in/out.</param>
        /// <param name="progress">Part of Progressive Notification.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, ScaleOptions options, ref ObservableCollection<float> progress)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(Color.Black).ZoomImage(ref e, options.X, options.Y);
            }, ref progress);

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Options for zooming in/out.</param>
        /// <param name="padColor">Background color that will be used when the frame is out of bounds.</param>
        /// <param name="progress">Part of Progressive Notification.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, ScaleOptions options, Color padColor, ref ObservableCollection<float> progress)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(padColor).ZoomImage(ref e, options.X, options.Y);
            }, ref progress);

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="scalePx">Pixels to zoom in/out (applies both to X and Y).</param>
        /// <param name="padColor">Background color that will be used when the frame is out of bounds.</param>
        /// <param name="progress">Part of Progressive Notification.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, int scalePx, Color padColor, ref ObservableCollection<float> progress)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(padColor).ZoomImage(ref e, scalePx, scalePx);
            }, ref progress);

        /// <summary>
        /// Zooms in/out to the part of the image.
        /// </summary>
        /// <param name="vid">Video to apply scaling.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="scalePx">Pixels to zoom in/out (applies both to X and Y).</param>
        /// <param name="progress">Part of Progressive Notification.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames Zoom(Video vid, StartEndTimestamp timestamp, int scalePx, ref ObservableCollection<float> progress)
            => vid.ModifyFramesInRange(timestamp, e =>
            {
                new ImageScalingHelper(Color.Black).ZoomImage(ref e, scalePx, scalePx);
            }, ref progress);
    }
}
