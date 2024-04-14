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
    /// Alias to method <see cref="BlackAndWhite.ApplyToPart(Video, StartEndTimestamp, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Timestamps where black &amp; white is applied.</param>
    /// <param name="cleanupMode">Settings for cleaning up files related to this task.</param>
    /// <returns>File names of generated files.</returns>
    public delegate GeneratedFileNames BlackAndWhiteApplyToPartAlias(
        Video vid,
        StartEndTimestamp timestamp,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// Alias to method <see cref="BlackAndWhite.ApplyToPart(Video, StartEndTimestamp, ref ObservableCollection{float}, DeleteGeneratedFiles)" />.
    /// </summary>
    /// <param name="vid">Input video.</param>
    /// <param name="timestamp">Timestamps where black &amp; white is applied.</param>
    /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
    /// <param name="cleanupMode">Settings for cleaning up files related to this task.</param>
    /// <returns>File names of generated files.</returns>
    public delegate GeneratedFileNames BlackAndWhiteApplyToPartAliasWithProgress(
        Video vid,
        StartEndTimestamp timestamp,
        ref ObservableCollection<float> progress,
        DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly);

    /// <summary>
    /// An effect for applying black &amp; white to a video.
    /// </summary>
    public static class BlackAndWhite
    {
        /// <summary>
        /// Applies black &amp; white effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Timestamps where black &amp; white is applied.</param>
        /// <param name="cleanupMode">Settings for cleaning up files related to this task.</param>
        /// <returns>File names of generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
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

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BlackWhite());
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
        /// Applies black &amp; white effect to a video.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamp">Timestamps where black &amp; white is applied.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for cleaning up files related to this task.</param>
        /// <returns>File names of generated files.</returns>
        public static GeneratedFileNames ApplyToPart(
            Video vid,
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

            string[] fileNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            float addBy = 100F / fileNames.Length;
            float thisCount = 0F;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            foreach (string file in fileNames)
            {
                Image image = Image.Load(file);
                image.Mutate(x => x.BlackWhite());
                image.Save(file);

                thisCount += addBy;
                progress.Add(thisCount);
            }

            progress.Add(0F);

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
