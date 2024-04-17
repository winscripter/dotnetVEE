using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Abstractions.UtilitySpecific.Exceptions;
using dotnetVEE.Computation.Options;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// A utility to manage appearance of text (for example, animate the
    /// text appearing and then disappearing).
    /// </summary>
    public class TextAppearanceUtility : IUtility, IGeneratesFiles
    {
        private readonly TextAppearanceAnimateOptions animationOptions;
        private readonly DeleteGeneratedFiles leaveGeneratedFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAppearanceUtility" /> class.
        /// </summary>
        /// <param name="animationOptions">Utility options.</param>
        /// <param name="leaveGeneratedFiles">Options for cleaning up generated files.</param>
        public TextAppearanceUtility(TextAppearanceAnimateOptions animationOptions, DeleteGeneratedFiles leaveGeneratedFiles = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            this.animationOptions = animationOptions;
            this.leaveGeneratedFiles = leaveGeneratedFiles;
        }

        /// <inheritdoc />
        public GeneratedFileNames? FileNames { get; set; }

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);

            int animationStartDuration = (int)(animationOptions.StartDuration.TotalSeconds * vid.FPS);
            int animationAppearDuration = (int)(animationOptions.AppearDuration.TotalSeconds * vid.FPS);
            int animationEndDuration = (int)(animationOptions.EndDuration.TotalSeconds * vid.FPS);

            int entireAnimationStart = (int)(animationOptions.Timestamps.Start.TotalSeconds * vid.FPS);
            int entireAnimationEnd = (int)(animationOptions.Timestamps.End.TotalSeconds * vid.FPS);

            int startPartStart = 0;
            int appearPartStart = startPartStart + animationStartDuration;
            int endPartStart = appearPartStart + animationAppearDuration - animationStartDuration;

            if ((animationStartDuration +
                 animationAppearDuration +
                 animationEndDuration) != (
                    entireAnimationEnd - entireAnimationStart
                ))
            {
                throw new UtilityException($"The animation timestamps for start, appear, and end mismatch with the actual animation start and end timestamp. Got total frame indexes of {animationStartDuration + animationAppearDuration + animationEndDuration}, expected exactly {entireAnimationEnd - entireAnimationStart}.");
            }

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                entireAnimationStart,
                entireAnimationEnd,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            float addBy = 100.0F / Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)).Length;
            float thisValue = addBy;

            // 0, 101 here because Enumerable.Range ranges 1 number less than 2nd parameter.
            List<float> zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();
            List<float> filteredZeroToOne = new List<float>();

            int gap = Math.Abs(animationStartDuration);
            int stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            var coordinates = new List<(int, int)>();

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            string[] fileNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName));

            int globalI = 0;

            for (int i = startPartStart; i < animationStartDuration; i++)
            {
                try
                {
                    Image<Rgba32> image = Image.Load<Rgba32>(fileNames[i + startPartStart]);

                    TextComputationOptions textOptions = animationOptions.ViewOptions;
                    Rgba32 brush = textOptions.Brush;
                    brush.A = (byte)(filteredZeroToOne[i] * byte.MaxValue);

                    textOptions.Brush = brush;

                    textOptions.DrawOnImage(image);

                    image.Save(fileNames[i]);
                    image.Dispose();

                    globalI++;
                }
                catch
                {
                    globalI = i;
                    break;
                }
            }

            filteredZeroToOne.Clear();
            zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();

            gap = Math.Abs(animationAppearDuration);
            stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            coordinates = new List<(int, int)>();

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            for (int i = appearPartStart; i < animationAppearDuration; i++)
            {
                try
                {
                    Image<Rgba32> image = Image.Load<Rgba32>(fileNames[i + appearPartStart]);

                    TextComputationOptions textOptions = animationOptions.ViewOptions;

                    textOptions.DrawOnImage(image);

                    image.Save(fileNames[i]);
                    image.Dispose();

                    globalI++;
                }
                catch
                {
                    globalI = i;
                    break;
                }
            }

            filteredZeroToOne.Clear();
            zeroToOne = Enumerable.Range(0, 101).Select(i => (float)(i / 100F)).ToList();

            gap = Math.Abs(animationEndDuration);
            stepSize = zeroToOne.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;

            coordinates = new List<(int, int)>();

            for (int j = 0; j < zeroToOne.Count; j += stepSize)
            {
                filteredZeroToOne.Add(zeroToOne[j]);
            }

            filteredZeroToOne.Reverse();

            for (int i = endPartStart; i < animationEndDuration + endPartStart; i++)
            {
                try
                {
                    Image<Rgba32> image = Image.Load<Rgba32>(fileNames[i]);

                    TextComputationOptions textOptions = animationOptions.ViewOptions;
                    Rgba32 brush = textOptions.Brush;
                    brush.A = (byte)(filteredZeroToOne[i - endPartStart] * byte.MaxValue);
                    textOptions.Brush = brush;
                    textOptions.DrawOnImage(image);

                    image.Save(fileNames[i]);
                    image.Dispose();

                    globalI++;
                }
                catch
                {
                    globalI = i;
                    break;
                }
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {animationOptions.Timestamps.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            FileNames = gfn;
            Cleaner.CleanAndRestore(gfn, leaveGeneratedFiles, vid.Path);

            progress.Add(100.0F);
        }
    }
}
