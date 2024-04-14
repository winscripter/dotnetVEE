using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects
{
    /// <summary>
    /// Options for the Bokeh Blur effect.
    /// </summary>
    /// <param name="Radius">The radius field for Bokeh Blur.</param>
    /// <param name="Components">The components field for Bokeh Blur.</param>
    /// <param name="Gamma">The gamma field for Bokeh Blur.</param>
    public record struct BokehBlurOptions(
        int Radius,
        int Components,
        float Gamma);

    /// <summary>
    /// Alias to method <see cref="BokehBlur.ApplyToPart(Video, StartEndTimestamp, BokehBlurOptions, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply effect on.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="options">Bokeh Blur options, i.e. Radius, Components, Gamma.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BokehBlurApplyToPartAlias(
        Video vid,
        StartEndTimestamp timestamp,
        BokehBlurOptions options,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BokehBlur.ApplyToPart(Video, StartEndTimestamp, BokehBlurOptions, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply effect on.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="options">Bokeh Blur options, i.e. Radius, Components, Gamma.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BokehBlurApplyToPartAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamp,
        BokehBlurOptions options,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BokehBlur.ApplyToPart(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply effect on.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BokehBlurApplyToPartAliasDefaultSettings(
        Video vid,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BokehBlur.ApplyToPart(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply effect on.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BokehBlurApplyToPartAliasDefaultSettingsWithProgress( // loooooooooooooong name
        Video vid,
        StartEndTimestamp timestamp,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect to apply bokeh &amp; blur to a video.
    /// </summary>
    public static class BokehBlur
    {
        /// <summary>
        /// Applies static Bokeh Blur effect to a video.
        /// </summary>
        /// <param name="vid">Video to apply effect on.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Bokeh Blur options, i.e. Radius, Components, Gamma.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
            Video vid,
            StartEndTimestamp timestamp,
            BokehBlurOptions options,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamp.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamp.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            ObservableCollection<float>? emptyProgress = new ObservableCollection<float>();

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref emptyProgress);
            emptyProgress = null;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BokehBlur(options.Radius, options.Components, options.Gamma));
                image.Save(file);
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Applies static Bokeh Blur effect to a video.
        /// </summary>
        /// <param name="vid">Video to apply effect on.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="options">Bokeh Blur options, i.e. Radius, Components, Gamma.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
            Video vid,
            StartEndTimestamp timestamp,
            BokehBlurOptions options,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamp.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamp.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0F);

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in files)
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BokehBlur(options.Radius, options.Components, options.Gamma));
                image.Save(file);

                thisCount += addBy;
                progress.Add(thisCount);
            }

            progress.Add(100F);

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Applies static Bokeh Blur effect to a video with default options for Bokeh Blur.
        /// </summary>
        /// <param name="vid">Video to apply effect on.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
            Video vid,
            StartEndTimestamp timestamp,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyToPart(vid, timestamp, new BokehBlurOptions(32, 2, 3F), cleanupMode);

        /// <summary>
        /// Applies static Bokeh Blur effect to a video with default options for Bokeh Blur.
        /// </summary>
        /// <param name="vid">Video to apply effect on.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
            Video vid,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyToPart(vid, timestamp, new BokehBlurOptions(32, 2, 3F), ref progress, cleanupMode);
    }
}
