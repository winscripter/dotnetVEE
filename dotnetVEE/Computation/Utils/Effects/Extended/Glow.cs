using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects.Extended
{
    /// <summary>
    /// An effect to apply glowing to a video.
    /// </summary>
    public static class Glow
    {
        /// <summary>
        /// Applies static glowing effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamps, e => e.Mutate(x => x.Glow()), cleanupMode);

        /// <summary>
        /// Applies static glowing effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamps, e => e.Mutate(x => x.Glow()), ref progress, cleanupMode);
    }
}
