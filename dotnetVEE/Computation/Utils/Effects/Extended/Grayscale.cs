using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects.Extended
{
    /// <summary>
    /// An effect for applying grayscaling to a video.
    /// </summary>
    public static class Grayscale
    {
        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="mode">Grayscaling mode.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            GrayscaleMode mode = GrayscaleMode.Bt709,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(mode)), cleanupMode);

        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="mode">Grayscaling mode.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            GrayscaleMode mode = GrayscaleMode.Bt709,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(mode)), ref progress, cleanupMode);

        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="amount">Grayscaling amount (BT.709 toning is used).</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float amount,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(amount)), cleanupMode);

        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="amount">Grayscaling amount (BT.709 toning is used).</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            float amount,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(amount)), ref progress, cleanupMode);

        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="amount">Grayscaling amount.</param>
        /// <param name="mode">Grayscaling mode.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float amount,
            GrayscaleMode mode = GrayscaleMode.Bt709,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(mode, amount)), cleanupMode);

        /// <summary>
        /// Applies static Grayscaling to an image.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="amount">Grayscaling amount (BT.709 toning is used).</param>
        /// <param name="mode">Grayscaling mode.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            float amount,
            GrayscaleMode mode = GrayscaleMode.Bt709,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => vid.ModifyFramesInRange(timestamp, e => e.Mutate(x => x.Grayscale(mode, amount)), ref progress, cleanupMode);
    }
}
