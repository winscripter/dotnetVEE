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
    /// Alias to method <see cref="ColorBlindness.ApplyPart(Video, ColorBlindnessMode, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="cbm">Type of the Color Blindness.</param>
    /// <param name="timestamp">Start and End timestamps for the effect to apply.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames ColorBlindnessApplyPartAlias(
        Video vid,
        ColorBlindnessMode cbm,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="ColorBlindness.ApplyPart(Video, ColorBlindnessMode, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="cbm">Type of the Color Blindness.</param>
    /// <param name="timestamp">Start and End timestamps for the effect to apply.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for the cleanup task.</param>
    /// <returns>File names for generated files.</returns>
    public delegate GeneratedFileNames ColorBlindnessApplyPartAliasWithProgress(
        Video vid,
        ColorBlindnessMode cbm,
        StartEndTimestamp timestamp,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect to apply Color Blindness to a video.
    /// </summary>
    public static class ColorBlindness
    {
        /// <summary>
        /// Adds a Color Blindness effect to a part of the video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="cbm">Type of the Color Blindness.</param>
        /// <param name="timestamp">Start and End timestamps for the effect to apply.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            ColorBlindnessMode cbm,
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

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    image.Mutate(x => x.ColorBlindness(cbm));
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
        /// Adds a Color Blindness effect to a part of the video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="cbm">Type of the Color Blindness.</param>
        /// <param name="timestamp">Start and End timestamps for the effect to apply.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        /// <returns>File names for generated files.</returns>
        public static GeneratedFileNames ApplyPart(
            Video vid,
            ColorBlindnessMode cbm,
            StartEndTimestamp timestamp,
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
                    image.Mutate(x => x.ColorBlindness(cbm));
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
    }
}
