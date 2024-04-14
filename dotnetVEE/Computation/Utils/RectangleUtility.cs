using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Options;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// A utility to draw a static rectangle on a video or frame.
    /// </summary>
    public class RectangleUtility : IUtility
    {
        private readonly StaticRectangleOptions _options;
        private readonly StartEndTimestamp _timestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleUtility" /> class.
        /// </summary>
        /// <param name="options">The options describing the visibility and look of the rectangle.</param>
        /// <param name="timestamp">The start and end timestamp, where the rectangle appears and vanishes.</param>
        public RectangleUtility(StaticRectangleOptions options, StartEndTimestamp timestamp)
        {
            _options = options;
            _timestamp = timestamp;
        }

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);

            int startIndex = (int)(_timestamp.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(_timestamp.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            float addBy = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)).Length / 100F;

            float currentProgress = 0.0F;

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                _options.DrawOn(image);
                image.Save(file);
                image.Dispose();
                progress.Add(currentProgress);
                currentProgress += addBy;
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            string o = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {_timestamp.Start.TotalSeconds} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            progress.Add(100.0F);
        }
    }
}
