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
    /// Alias to method <see cref="Brightness.ApplyPart(Video, float, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="brightness">Brightness value. 0 - completely dark; 1 - default; 2 - completely light.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyPartAlias(
        Video vid,
        float brightness,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Brightness.ApplyPart(Video, float, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="brightness">Brightness value. 0 - completely dark; 1 - default; 2 - completely light.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyPartAliasWithProgress(
        Video vid,
        float brightness,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Brightness.ApplyAscending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyAscendingAlias(
        Video vid,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Brightness.ApplyAscending(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyAscendingAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Brightness.ApplyDescending(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyDescendingAlias(
        Video vid,
        StartEndTimestamp timestamps,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="Brightness.ApplyDescending(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleaning task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames BrightnessApplyDescendingAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamps,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect that adds brightness to a video.
    /// </summary>
    public static class Brightness
    {
        /// <summary>
        /// Applies brightness effect to a part of video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="brightness">Brightness value. 0 - completely dark; 1 - default; 2 - completely light.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            float brightness,
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

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Brightness(brightness));
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
        /// Applies brightness effect to a part of video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="brightness">Brightness value. 0 - completely dark; 1 - default; 2 - completely light.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            float brightness,
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

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.Brightness(brightness));
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
        /// Applies ascending brightness effect to a part of video,
        /// starting with 0 (completely dark) to 1 (normal).
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
                    image.Mutate(x => x.Brightness(filteredZeroToOne[i]));
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
        /// Applies ascending brightness effect to a part of video,
        /// starting with 0 (completely dark) to 1 (normal).
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
                    image.Mutate(x => x.Brightness(filteredZeroToOne[i]));
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
        /// Applies ascending brightness effect to a part of video,
        /// starting with 1 (normal) to 0 (completely dark).
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
                    image.Mutate(x => x.Brightness(filteredZeroToOne[i]));
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
        /// Applies ascending brightness effect to a part of video,
        /// starting with 1 (normal) to 0 (completely dark).
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
                    image.Mutate(x => x.Brightness(filteredZeroToOne[i]));
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
