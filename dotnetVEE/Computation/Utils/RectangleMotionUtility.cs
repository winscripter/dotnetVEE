using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation.Calculations;
using dotnetVEE.Computation.Options;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// Adds a rectangle that can move to a video.
    /// </summary>
    public class RectangleMotionUtility : IGeneratesFiles
    {
        private readonly RectangleMotionOptions options;
        private readonly DeleteGeneratedFiles leaveGeneratedFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleMotionUtility" /> class.
        /// </summary>
        /// <param name="options">Options describing the rectangle's look and its movement.</param>
        /// <param name="leaveGeneratedFiles">Options when it comes to deleting generated files.</param>
        public RectangleMotionUtility(RectangleMotionOptions options, DeleteGeneratedFiles leaveGeneratedFiles = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            this.options = options;
            this.leaveGeneratedFiles = leaveGeneratedFiles;
        }

        /// <inheritdoc />
        public GeneratedFileNames? FileNames { get; set; } = null;

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);

            int startIndex = (int)(options.MotionOptions.StartTimestamp.TotalSeconds * vid.FPS);
            int endIndex = (int)(options.MotionOptions.EndTimestamp.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            var coords = BresenhamsLineDrawing.GetLineCoordinates(
                options.MotionOptions.Start.X,
                options.MotionOptions.Start.Y,
                options.MotionOptions.End.X,
                options.MotionOptions.End.Y
            );

            int gap = Math.Abs(endIndex - startIndex);
            int stepSize = coords.Count / gap;

            stepSize = stepSize == 0 ? 1 : stepSize;
            
            var coordinates = new List<(int, int)>();

            for (int j = 0; j < coords.Count; j += stepSize)
            {
                coordinates.Add(coords[j]);
            }

            float addBy = 100.0F / Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)).Length;
            float thisValue = addBy;
            int coordinateIndex = 0;

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                try
                {
                    (int x, int y) thisCoordinate = coordinates[coordinateIndex++];

                    Image img = Image.Load(file);
                    RectangleF borders = new RectangleF(
                        new PointF(coordinates[coordinateIndex].Item1, coordinates[coordinateIndex].Item2),
                        this.options.ObjectOptions.RectangleSize
                    );

                    img.Mutate(x => x.Draw(
                        this.options.ObjectOptions.RectangleColor.ToSixLaborsRgba32(),
                        this.options.ObjectOptions.RectangleBorderThickness,
                        borders));

                    progress.Add(thisValue);
                    thisValue += addBy;

                    img.Save(file);
                    img.Dispose();

                    continue;
                }
                catch
                {
                    progress.Add(100.0F);
                    break;
                }
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {options.MotionOptions.StartTimestamp.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            FileNames = gfn;
            Cleaner.CleanAndRestore(gfn, leaveGeneratedFiles, vid.Path);

            progress.Add(100.0F);
        }
    }
}
