using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects.Extended
{
    /// <summary>
    /// An effect to apply hue to a video.
    /// </summary>
    public static class Hue
    {
        /// <summary>
        /// Applies the Hue effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="degree">Amount of degrees for the Hue effect.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float degree,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Hue(degree)), cleanupMode);

        /// <summary>
        /// Applies the Hue effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="degree">Amount of degrees for the Hue effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float degree,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Hue(degree)), ref progress, cleanupMode);
    }
}
