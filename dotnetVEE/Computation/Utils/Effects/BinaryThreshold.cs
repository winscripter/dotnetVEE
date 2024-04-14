using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils.Effects
{
    /// <summary>
    /// Options for applying Binary Threshold for a video.
    /// </summary>
    /// <param name="Threshold">Amount of threshold.</param>
    /// <param name="Mode">Binary threshold mode.</param>
    public record struct BinaryThresholdOptions(
        float Threshold,
        BinaryThresholdMode Mode = BinaryThresholdMode.Luminance);

    /// <summary>
    /// Options for applying Binary Threshold when it comes to effect animation.
    /// </summary>
    /// <param name="Mode">Binary threshold mode.</param>
    public record struct BinaryThresholdAnimatableOptions(
        BinaryThresholdMode Mode = BinaryThresholdMode.Luminance);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimatePart(Video, BinaryThresholdOptions, StartEndTimestamp, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimatePartAlias(
        Video vid,
        BinaryThresholdOptions options,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimateAscending(Video, BinaryThresholdAnimatableOptions, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimateAscendingAlias(
        Video vid,
        BinaryThresholdOptions options,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimateDescending(Video, BinaryThresholdAnimatableOptions, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimateDescendingAlias(
        Video vid,
        BinaryThresholdOptions options,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimatePart(Video, BinaryThresholdOptions, StartEndTimestamp, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimatePartAliasWithProgress(
        Video vid,
        BinaryThresholdOptions options,
        StartEndTimestamp timestamps,
        StartEndTimestamp part,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimateDescending(Video, BinaryThresholdAnimatableOptions, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimateDescendingAliasWithProgress(
        Video vid,
        BinaryThresholdAnimatableOptions options,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BinaryThreshold.AnimateAscending(Video, BinaryThresholdAnimatableOptions, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Video to apply this effect for.</param>
    /// <param name="options">Options for computing Binary Threshold.</param>
    /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
    /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
    public delegate GeneratedFileNames BinaryThresholdAnimateAscendingAliasWithProgress(
        Video vid,
        BinaryThresholdAnimatableOptions options,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect for applying binary threshold to an image.
    /// </summary>
    public static class BinaryThreshold
    {
        /// <summary>
        /// Adds Binary Threshold effect to a part of a video.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimatePart(
            Video vid,
            BinaryThresholdOptions options,
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
                image.Mutate(x => x.BinaryThreshold(options.Threshold, options.Mode));
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
        /// Creates an effect where between the selected timestamps of the video,
        /// the binary threshold is applied. Starting with 0, the value slowly
        /// ascends until it hits 1 (full binary threshold). Speed is automatically
        /// adjusted based on how long the effect lasts for.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimateAscending(
            Video vid,
            BinaryThresholdAnimatableOptions options,
            StartEndTimestamp timestamps,
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

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();
            List<float> filteredZeroToOne = new List<float>();

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int gap = Math.Abs(files.Length);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BinaryThreshold(filteredZeroToOne[i], options.Mode));
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
        /// Creates an effect where between the selected timestamps of the video,
        /// the binary threshold is applied. Starting with 1, the value slowly
        /// descends until it hits 0 (min. binary threshold). Speed is automatically
        /// adjusted based on how long the effect lasts for.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimateDescending(
            Video vid,
            BinaryThresholdAnimatableOptions options,
            StartEndTimestamp timestamps,
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

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();
            List<float> filteredZeroToOne = new List<float>();

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int gap = Math.Abs(files.Length);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            filteredZeroToOne.Reverse();

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BinaryThreshold(filteredZeroToOne[i], options.Mode));
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
        /// Adds Binary Threshold effect to a part of a video.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="part">Timestamps selecting start &amp; end timestamps indicating where to apply effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimatePart(
            Video vid,
            BinaryThresholdOptions options,
            StartEndTimestamp timestamps,
            StartEndTimestamp part,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0.0F);

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

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / files.Length;
            float thisCount = 0.0F;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in files)
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BinaryThreshold(options.Threshold, options.Mode));
                image.Save(file);

                thisCount += addBy;
                progress.Add(thisCount);
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

        /// <summary>
        /// Creates an effect where between the selected timestamps of the video,
        /// the binary threshold is applied. Starting with 0, the value slowly
        /// ascends until it hits 1 (full binary threshold). Speed is automatically
        /// adjusted based on how long the effect lasts for.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimateAscending(
            Video vid,
            BinaryThresholdAnimatableOptions options,
            StartEndTimestamp timestamps,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0.0F);

            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0.0F);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();
            List<float> filteredZeroToOne = new List<float>();

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int gap = Math.Abs(files.Length);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            float addBy = 100F / files.Length;
            float thisProgress = 0.0F;

            var coordinates = new List<(int, int)>();

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BinaryThreshold(filteredZeroToOne[i], options.Mode));
                    image.Save(files[i]);

                    thisProgress += addBy;
                    progress.Add(thisProgress);
                }
                catch
                {
                    break;
                }
            }

            progress.Add(100F);

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Creates an effect where between the selected timestamps of the video,
        /// the binary threshold is applied. Starting with 1, the value slowly
        /// descends until it hits 0 (min. binary threshold). Speed is automatically
        /// adjusted based on how long the effect lasts for.
        /// </summary>
        /// <param name="vid">Video to apply this effect for.</param>
        /// <param name="options">Options for computing Binary Threshold.</param>
        /// <param name="timestamps">Timestamps selecting start &amp; end timestamps indicating where to apply tasks.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <returns>File names for generated files. May <i>sometimes</i> be useful.</returns>
        /// <exception cref="ArgumentException">Exception thrown when arguments have an invalid value.</exception>
        public static GeneratedFileNames AnimateDescending(
            Video vid,
            BinaryThresholdAnimatableOptions options,
            StartEndTimestamp timestamps,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0.0F);

            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            progress.Add(0.0F);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();
            List<float> filteredZeroToOne = new List<float>();

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int gap = Math.Abs(files.Length);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            float addBy = 100F / files.Length;
            float thisProgress = 0.0F;

            var coordinates = new List<(int, int)>();

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            filteredZeroToOne.Reverse();

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.BinaryThreshold(filteredZeroToOne[i], options.Mode));
                    image.Save(files[i]);

                    thisProgress += addBy;
                    progress.Add(thisProgress);
                }
                catch
                {
                    break;
                }
            }

            progress.Add(100F);

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
