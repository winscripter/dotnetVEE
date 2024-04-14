using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Abstractions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using System.Collections.ObjectModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace dotnetVEE.Computation.Utils.Effects
{
    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyPart(Video, StartEndTimestamp, float, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Start &amp; End timestamp for the effect to apply.</param>
    /// <param name="saturation">Amount of saturation for the effect.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyPartAlias(
        Video vid,
        StartEndTimestamp timestamp,
        float saturation,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyPart(Video, StartEndTimestamp, float, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Start &amp; End timestamp for the effect to apply.</param>
    /// <param name="saturation">Amount of saturation for the effect.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyPartAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamp,
        float saturation,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyAscending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyAscendingAlias(
        Video vid,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyAscending(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyAscendingAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyDescending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyDescendingAlias(
        Video vid,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Saturate.ApplyDescending(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames SaturateApplyDescendingAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect to apply saturation to a video.
    /// </summary>
    public static class Saturate
    {
        /// <summary>
        /// Applies static saturation effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamp for the effect to apply.</param>
        /// <param name="saturation">Amount of saturation for the effect.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float saturation,
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

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Saturate(saturation));
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
        /// Applies static saturation effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Start &amp; End timestamp for the effect to apply.</param>
        /// <param name="saturation">Amount of saturation for the effect.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            StartEndTimestamp timestamp,
            float saturation,
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

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Saturate(saturation));
                    image.Save(files[i]);

                    thisCount += addBy;
                    progress.Add(thisCount);
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

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, vid.Path);

            return gfn;
        }

        /// <summary>
        /// Applies ascending saturation effect to a part of video,
        /// starting with 0 all the way to 1.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyAscending(
            Video vid,
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

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(f => (float)f / 100F).ToList();
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
                    image.Mutate(x => x.Saturate(filteredZeroToOne[i]));
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
        /// Applies ascending saturation effect to a part of video,
        /// starting with 0 all the way to 1.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyAscending(
            Video vid,
            StartEndTimestamp timestamps,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(f => (float)f / 100F).ToList();
            List<float> filteredZeroToOne = new List<float>();

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int gap = Math.Abs(files.Length);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Saturate(filteredZeroToOne[i]));
                    image.Save(files[i]);

                    thisCount += addBy;
                    progress.Add(thisCount);
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
        /// Applies descending saturation effect to a part of video,
        /// starting with 1 all the way to 0.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyDescending(
            Video vid,
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

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(f => (float)f / 100F).ToList();
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
                    image.Mutate(x => x.Contrast(filteredZeroToOne[i]));
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
        /// Applies descending saturation effect to a part of video,
        /// starting with 1 all the way to 0.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyDescending(
            Video vid,
            StartEndTimestamp timestamps,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            List<float> zeroToOne = Enumerable.Range(0, 101).Select(f => (float)f / 100F).ToList();
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

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Saturate(filteredZeroToOne[i]));
                    image.Save(files[i]);

                    thisCount += addBy;
                    progress.Add(thisCount);
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
