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
    /// Alias to method <see cref="BoxBlur.ApplyPart(Video, int, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="radius">Amount of radius for the effect.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyPartAlias(
        Video vid,
        int radius,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BoxBlur.ApplyPart(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyPartAliasDefaultRadius(
        Video vid,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BoxBlur.ApplyPart(Video, int, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="radius">Amount of radius for the effect.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyPartAliasWithProgress(
        Video vid,
        int radius,
        StartEndTimestamp timestamp,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BoxBlur.ApplyPart(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />/
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyPartAliasWithProgressDefaultSettings( // loooooooooooooooooooooooooooong
        Video vid,
        StartEndTimestamp timestamp,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BoxBlur.ApplyAscending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Animation start &amp; end timestamp.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyAscendingAlias(
        Video vid,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BoxBlur.ApplyDescending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Animation start &amp; end timestamp.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BoxBlurApplyDescendingAlias(
        Video vid,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect to add Box Blur effect to a video.
    /// </summary>
    public static class BoxBlur
    {
        /// <summary>
        /// Applies static Box Blur effect to a part of a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="radius">Amount of radius for the effect.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            int radius,
            StartEndTimestamp timestamp,
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
                image.Mutate(x => x.BoxBlur(radius));
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
        /// Applies static Box Blur effect to a part of a video with the default radius being <b>7</b>.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, 7, timestamp, cleanupMode);

        /// <summary>
        /// Applies static Box Blur effect to a part of a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="radius">Amount of radius for the effect.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            int radius,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0F);

            int startIndex = (int)(timestamp.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamp.End.TotalSeconds * vid.FPS);

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

            foreach (string file in files)
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BoxBlur(radius));
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
        /// Applies static Box Blur effect to a part of a video with the default radius being <b>7</b>.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamps for the effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
            => ApplyPart(vid, 7, timestamp, ref progress, cleanupMode);

        /// <summary>
        /// Animates an ascending Box Blur effect starting with
        /// 0% all the way to 100%. Speed is automatically adjusted
        /// based on the length of the animation.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Animation start &amp; end timestamp.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyAscending(
            Video vid,
            StartEndTimestamp timestamp,
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

            List<int> zeroToOne = Enumerable.Range(0, 101).ToList();
            List<int> filteredZeroToOne = new List<int>();

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
                    image.Mutate(x => x.BoxBlur(filteredZeroToOne[i]));
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

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Animates a descending Box Blur effect starting with
        /// 100% all the way to 0%. Speed is automatically adjusted
        /// based on the length of the animation.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Animation start &amp; end timestamp.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyDescending(
            Video vid,
            StartEndTimestamp timestamp,
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

            List<int> zeroToOne = Enumerable.Range(0, 101).ToList();
            List<int> filteredZeroToOne = new List<int>();

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
                    image.Mutate(x => x.BoxBlur(filteredZeroToOne[i]));
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

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }
    }
}
