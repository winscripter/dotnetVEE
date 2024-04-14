using dotnetVEE.Abstractions;
using dotnetVEE.Computation.Options;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// A utility to add text to a video within given timestamps.
    /// </summary>
    public class AddText : IUtility
    {
        private readonly TextComputationOptions options;
        private readonly StartEndTimestamp startEndTimestamp;
        private readonly bool leaveGeneratedFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddText" /> class.
        /// </summary>
        /// <param name="options">Computation options for the text before drawing.</param>
        /// <param name="startEndTimestamp">Start and end timestamp, where the text appears and vanishes.</param>
        /// <param name="leaveGeneratedFile">
        /// When the text is added to a video, dotnetVEE temporarily generates
        /// a video file with a random file name because FFmpeg doesn't support
        /// changing the video indirectly, so there has to be a copy. When the value of this
        /// parameter is set to <see langword="TRUE" />, the generated file won't be deleted;
        /// otherwise it will (which is the default). The file name of this generated file
        /// is stored in the property <see cref="OutputVideoFileName" /> (which will always
        /// be null if this parameter has a value of <see langword="FALSE" />, which is the default).
        /// </param>
        public AddText(TextComputationOptions options, StartEndTimestamp startEndTimestamp, bool leaveGeneratedFile = false)
        {
            this.options = options;
            this.startEndTimestamp = startEndTimestamp;
            this.leaveGeneratedFile = leaveGeneratedFile;
        }

        /// <summary>
        /// The file name of the output video. This is <see langword="NULL" /> if the
        /// <c>leaveGeneratedFile</c> boolean to the constructor isn't passed or is
        /// set to <see langword="FALSE" />.
        /// </summary>
        public string? OutputVideoFileName { get; private set; }

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);

            int startIndex = (int)(startEndTimestamp.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(startEndTimestamp.End.TotalSeconds * vid.FPS);

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
                options.DrawOnImage(image);
                image.Save(file);
                image.Dispose();

                thisCount += addBy;
                progress.Add(thisCount);
            }

            progress.Add(100F);

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            string o = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {startEndTimestamp.Start.TotalSeconds.SafeStringify()} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            if (!leaveGeneratedFile)
            {
                if (File.Exists(vid.Path))
                {
                    File.Delete(vid.Path);
                }

                File.Move(outputFileName, vid.Path); // rename it to the original filename, technically overwrite existing one
                Directory.Delete(dirName, true);
                this.OutputVideoFileName = outputFileName;
            }
            else
            {
                this.OutputVideoFileName = null;
            }

            progress.Add(100.0F);
        }
    }
}
