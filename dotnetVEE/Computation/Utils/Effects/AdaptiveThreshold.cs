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
    /// Alias to method <see cref="AdaptiveThreshold.AnimatePartEasy(Video, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames AdaptiveThresholdAnimatePartEasyAlias(
        Video vid,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="AdaptiveThreshold.AnimatePart(Video, float, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="thresholdLimit">Custom threshold limit. Must range between 0 and 1.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames AdaptiveThresholdAnimatePartAlias(
        Video vid,
        float thresholdLimit,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="AdaptiveThreshold.AnimatePartEasy(Video, StartEndTimestamp, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    /// <remarks>
    /// The difference between this method and <see cref="AdaptiveThreshold.AnimatePartEasy(Video, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" /> is that the former supports progress notification, while the latter does not.
    /// </remarks>
    public delegate GeneratedFileNames AdaptiveThresholdAnimatePartEasyAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="AdaptiveThreshold.AnimatePart(Video, float, StartEndTimestamp, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="thresholdLimit">Custom threshold limit. Must range between 0 and 1.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    /// <remarks>
    /// The difference between this method and <see cref="AdaptiveThreshold.AnimatePart(Video, float, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" /> is that the former supports progress notification, while the latter does not.
    /// </remarks>
    public delegate GeneratedFileNames AdaptiveThresholdAnimatePartAliasWithProgress(
        Video vid,
        float thresholdLimit,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect for applying adaptive threshold to an image.
    /// </summary>
    public static class AdaptiveThreshold
    {
        /// <summary>
        /// Adds Adaptive Threshold effect to a part of a video using default settings.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimatePartEasy(
            Video vid,
            StartEndTimestamp timestamps,
            StartEndTimestamp part,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(part.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(part.End.TotalSeconds * vid.FPS);

            int vStartIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int vEndIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            if (startIndex > vStartIndex)
            {
                throw new ArgumentException($"Start timestamp is greater than the whole selected timestamp.", nameof(startIndex));
            }

            if (endIndex > vEndIndex)
            {
                throw new ArgumentException($"End timestamp is greater than the whole selected timestamp.", nameof(endIndex));
            }

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
                image.Mutate(x => x.AdaptiveThreshold());
                image.Save(file);
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
        /// Adds Adaptive Threshold effect to a part of a video.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="thresholdLimit">Custom threshold limit. Must range between 0 and 1.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimatePart(
            Video vid,
            float thresholdLimit,
            StartEndTimestamp timestamps,
            StartEndTimestamp part,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(part.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(part.End.TotalSeconds * vid.FPS);

            int vStartIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int vEndIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            if (startIndex > vStartIndex)
            {
                throw new ArgumentException($"Start timestamp is greater than the whole selected timestamp.", nameof(startIndex));
            }

            if (endIndex > vEndIndex)
            {
                throw new ArgumentException($"End timestamp is greater than the whole selected timestamp.", nameof(endIndex));
            }

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
                image.Mutate(x => x.AdaptiveThreshold(thresholdLimit));
                image.Save(file);
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
        /// Adds Adaptive Threshold effect to a part of a video.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        /// <remarks>
        /// The difference between this method and <see cref="AnimatePartEasy(Video, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" /> is that the former supports progress notification, while the latter does not.
        /// </remarks>
        public static GeneratedFileNames AnimatePartEasy(
            Video vid,
            StartEndTimestamp timestamps,
            StartEndTimestamp part,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(part.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(part.End.TotalSeconds * vid.FPS);

            int vStartIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int vEndIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            if (startIndex > vStartIndex)
            {
                throw new ArgumentException($"Start timestamp is greater than the whole selected timestamp.", nameof(startIndex));
            }

            if (endIndex > vEndIndex)
            {
                throw new ArgumentException($"End timestamp is greater than the whole selected timestamp.", nameof(endIndex));
            }

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0.0F);

            string[] fileNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / fileNames.Length;
            float thisProgress = 0.0F;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.AdaptiveThreshold());
                image.Save(file);

                thisProgress += addBy;
                progress.Add(thisProgress);
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            progress.Add(0.0F);

            return gfn;
        }

        /// <summary>
        /// Adds Adaptive Threshold effect to a part of a video.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="thresholdLimit">Custom threshold limit. Must range between 0 and 1.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        /// <remarks>
        /// The difference between this method and <see cref="AnimatePart(Video, float, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" /> is that the former supports progress notification, while the latter does not.
        /// </remarks>
        public static GeneratedFileNames AnimatePart(
            Video vid,
            float thresholdLimit,
            StartEndTimestamp timestamps,
            StartEndTimestamp part,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(part.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(part.End.TotalSeconds * vid.FPS);

            int vStartIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int vEndIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            if (startIndex > vStartIndex)
            {
                throw new ArgumentException($"Start timestamp is greater than the whole selected timestamp.", nameof(startIndex));
            }

            if (endIndex > vEndIndex)
            {
                throw new ArgumentException($"End timestamp is greater than the whole selected timestamp.", nameof(endIndex));
            }

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0.0F);

            string[] fileNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / fileNames.Length;
            float thisProgress = 0.0F;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.AdaptiveThreshold(thresholdLimit));
                image.Save(file);

                thisProgress += addBy;
                progress.Add(thisProgress);
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            progress.Add(0.0F);

            return gfn;
        }
    }
}
