using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects
{
    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasR64(
        Video vid,
        StartEndTimestamp timestamps,
        Rgba64 color,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasRv(
        Video vid,
        StartEndTimestamp timestamps,
        RgbaVector color,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasRrf(
        Video vid,
        StartEndTimestamp timestamps,
        RgbaRF color,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasR32(
        Video vid,
        StartEndTimestamp timestamps,
        Rgba32 color,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasC(
        Video vid,
        StartEndTimestamp timestamps,
        Color color,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasWithProgressRv(
        Video vid,
        StartEndTimestamp timestamps,
        RgbaVector color,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasWithProgressR32(
        Video vid,
        StartEndTimestamp timestamps,
        Rgba32 color,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasWithProgressR64(
        Video vid,
        StartEndTimestamp timestamps,
        Rgba64 color,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasWithProgressRrf(
        Video vid,
        StartEndTimestamp timestamps,
        RgbaRF color,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Applies background color to a video within given two start &amp; end timestamps.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
    /// <param name="color">Background color.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning generated files.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BackgroundColorApplyPartAliasWithProgressC(
        Video vid,
        StartEndTimestamp timestamps,
        Color color,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect for applying background color to a video.
    /// </summary>
    public static class BackgroundColor
    {
        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Rgba64 color,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)color, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            RgbaVector color,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)new Rgba32(color.R, color.G, color.B, color.A), cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            RgbaRF color,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, color.ToSixLaborsRgba32(), cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Rgba32 color,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)color, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Color color,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            ObservableCollection<float>? emptyProgress = new ObservableCollection<float>();

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref emptyProgress);
            emptyProgress = null;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BackgroundColor(color));
                    image.Save(files[i]);
                }
                catch
                {
                    break;
                }
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            RgbaVector color,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)new Rgba32(color.R, color.G, color.B, color.A), ref progress, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Rgba64 color,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)color, ref progress, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            RgbaRF color,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)color.ToSixLaborsRgba32(), ref progress, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Rgba32 color,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, timestamps, (Color)color, ref progress, cleanupMode);

        /// <summary>
        /// Applies background color to a video within given two start &amp; end timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; timestamps where the effect starts to apply.</param>
        /// <param name="color">Background color.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning generated files.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamps,
            Color color,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0F);

            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0F);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BackgroundColor(color));
                    image.Save(files[i]);

                    thisCount += addBy;
                    progress.Add(addBy);
                }
                catch
                {
                    break;
                }
            }

            progress.Add(100.0F);

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }
    }
}
