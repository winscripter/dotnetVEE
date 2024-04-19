using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.Exceptions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation;
using dotnetVEE.Computation.Audio;
using dotnetVEE.Computation.Query;
using dotnetVEE.Computation.Utils;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;
using System.Numerics;

namespace dotnetVEE
{
    /// <summary>
    /// Action for frames while looping through all or selected
    /// frames in the video.
    /// </summary>
    /// <param name="frame">A given frame throughout the video.</param>
    public delegate void FrameAction(Image frame);

    /// <summary>
    /// Action for frames while looping through all frames in the video.
    /// </summary>
    /// <param name="frameName">Path to the frame file.</param>
    /// <param name="index">Frame index.</param>
    public delegate void FrameLoopAction(string frameName, int index);

    /// <summary>
    /// Represents information about a video.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Frames Per Second (FPS) for this video.
        /// </summary>
        public float FPS { get; private protected init; }

        /// <summary>
        /// Rounded Frames Per Second (FPS) for this video. For
        /// example, if <see cref="FPS" /> returns 29.96, this should
        /// return 30.
        /// </summary>
        public int RoundedFPS { get; private protected init; }

        /// <summary>
        /// A count of frames in the video.
        /// </summary>
        public int FrameCount { get; private protected init; }

        /// <summary>
        /// Resolution of the video.
        /// </summary>
        public Vector2 Resolution { get; private protected init; }

        /// <summary>
        /// Path to the video file.
        /// </summary>
        public string Path { get; private protected init; } = string.Empty;

        private Video()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video" /> class. To
        /// automate this process and set all properties to their appropriate
        /// values except the video path which is mandatory, please use the
        /// <see cref="Create(string)" /> method. Keep in mind that setting values
        /// to their incorrect values could lead to unpredictable behavior, so it's
        /// generally safe to use <see cref="Create(string)" /> method to set correct
        /// values based on actual video metadata.
        /// </summary>
        /// <param name="FPS">The Frames Per Second of this video.</param>
        /// <param name="roundedFPS">The rounded Frames Per Second of this video.</param>
        /// <param name="frameCount">The count of frames of this video.</param>
        /// <param name="resolution">The resolution of this video.</param>
        /// <param name="path">Path to this video.</param>
        public Video(float FPS, int roundedFPS, int frameCount, Vector2 resolution, string path)
        {
            this.FPS = FPS;
            RoundedFPS = roundedFPS;
            FrameCount = frameCount;
            Resolution = resolution;
            Path = path;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Video" /> from a video file.
        /// </summary>
        /// <param name="videoPath">Video file.</param>
        /// <returns>A new instance of <see cref="Video" /> containing information from this video.</returns>
        /// <exception cref="FileNotFoundException">Exception thrown when a video file can't be found.</exception>
        public static Video Create(string videoPath)
        {
            if (!File.Exists(videoPath))
            {
                throw new FileNotFoundException($"Cannot find \"{videoPath}\".");
            }

            float fps = VideoDataQuery.GetFpsCount(videoPath);

            var video = new Video
            {
                FPS = fps,
                FrameCount = VideoDataQuery.GetFrameCount(videoPath),
                Path = videoPath,
                Resolution = VideoDataQuery.GetVideoDimensions(videoPath),
                RoundedFPS = (int)Math.Round(fps)
            };

            return video;
        }

        /// <summary>
        /// Gets the frame index at a given timestamp. For example, if the
        /// video has 30 FPS and the timestamp is exactly 1 minute and 0 seconds,
        /// this method will return 1800, because that's the frame index for the
        /// given timestamp. Why 1800? The algorithm is <c>fps * timestamp.totalSeconds</c>.
        /// The totalSeconds is 60, because 1 minute is 60 seconds, so by multiplying 30
        /// by 60, you get 1800, which is the frame index.
        /// </summary>
        /// <remarks>
        /// Frame indexes returned by this method <i>start with 1</i>.
        /// </remarks>
        /// <param name="timestamp">The timestamp where the frame index should be returned.</param>
        /// <returns>Frame index of this video based on the timestamp.</returns>
        public double GetFrameIndexAtTimestamp(TimeSpan timestamp) => timestamp.TotalSeconds * FPS;

        /// <summary>
        /// Loops through all frames within the given timestamps,
        /// and calls <paramref name="action" /> for each frame.
        /// </summary>
        /// <param name="timestamps">Start &amp; End timestamps for the effect.</param>
        /// <param name="action">Method that will be called for each frame seen.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public GeneratedFileNames ModifyFramesInRange(
            StartEndTimestamp timestamps,
            FrameAction action,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            int startIndex = (int)(timestamps.Start.TotalSeconds * this.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * this.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            ObservableCollection<float>? emptyProgress = new ObservableCollection<float>();

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(this, ref emptyProgress);

            emptyProgress = null;

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            string[] files = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), dirName));
            // look above after Directory.GetFiles
            // -- System.IO is NECESSARY ! --

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    action(image);
                    image.Save(files[i]);
                    image.Dispose();
                }
                catch
                {
                    break;
                }
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                this.Path.Contains('.') ? this.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{this.Path}\" -itsoffset {timestamps.Start.TotalSeconds.SafeStringify()} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, System.IO.Path.GetFullPath(dirName), outputFileName, System.IO.Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, this.Path);

            return gfn;
        }

        /// <summary>
        /// Loops through all frames within the given timestamps,
        /// and calls <paramref name="action" /> for each frame.
        /// </summary>
        /// <param name="timestamps">Start &amp; End timestamps for the effect.</param>
        /// <param name="action">Method that will be called for each frame seen.</param>
        /// <param name="progress">An <see cref="ObservableCollection{T}" /> where progress notification goes.</param>
        /// <param name="cleanupMode">Settings for the cleaning task.</param>
        /// <returns>File names for generated files.</returns>
        public GeneratedFileNames ModifyFramesInRange(
            StartEndTimestamp timestamps,
            FrameAction action,
            ref ObservableCollection<float> progress,
            DeleteGeneratedFiles cleanupMode = DeleteGeneratedFiles.FramesDirectoryOnly)
        {
            progress.Add(0F);

            int startIndex = (int)(timestamps.Start.TotalSeconds * this.FPS);
            int endIndex = (int)(timestamps.End.TotalSeconds * this.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(this, ref progress);

            progress.Add(100F);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            string[] files = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), dirName));
            // look above after Directory.GetFiles
            // -- System.IO is NECESSARY ! --

            float addBy = 100F / files.Length;
            float thisCount = 0F;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Image image = Image.Load(files[i]);
                    action(image);
                    image.Save(files[i]);
                    image.Dispose();

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
                this.Path.Contains('.') ? this.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{this.Path}\" -itsoffset {timestamps.Start.TotalSeconds.SafeStringify()} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, System.IO.Path.GetFullPath(dirName), outputFileName, System.IO.Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, cleanupMode, this.Path);

            return gfn;
        }

        /// <summary>
        /// Loops through each frame in a video and calls <paramref name="frameAction" /> on it. Each
        /// frame is deleted after calling <paramref name="frameAction" />.
        /// </summary>
        /// <param name="frameAction">Action for every frame.</param>
        /// <param name="into">Temporary directory where frames are extracted. Defaults to the current one.</param>
        /// <remarks>
        ///     <b>Note:</b> This method can be <i>very</i> slow especially when processing a long video.
        ///                  The only upside is that it is memory efficient and only processes 1 frame at
        ///                  a time before it is deleted.
        /// </remarks>
        public void ForEachFrame(FrameLoopAction frameAction, string into = ".")
        {
            for (int i = 0; i < FrameCount; i++)
            {
                string fileName = System.IO.Path.Combine(into, $"frame_{i}.png");

                this.ExtractFrameAt(i, fileName);
                frameAction(fileName, i);

                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Gets information about an audio inside this video.
        /// </summary>
        /// <returns>Information about an audio from a video.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the audio inside a video seems to be invalid or unsupported.</exception>
        public AudioInfo GetAudioInformationInsideVideo()
        {
            string fileName = RandomPathGenerator.GenerateDirectoryNameV1();

            AudioKind ak = AudioManager.AutomatedExtractAudio(this, fileName)
                           ?? throw new InvalidOperationException("The video file seems to contain invalid or unsupported audio format.");

            return new AudioInfo($"{fileName}.{ak.ToString().ToLower()}");
        }

        /// <summary>
        /// Gets information about an audio inside this video.
        /// </summary>
        /// <returns>Information about an audio from a video.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the audio inside a video seems to be invalid or unsupported.</exception>
        /// <remarks>
        ///     <b>Note:</b> This method is similar to <see cref="GetAudioInformationInsideVideo" />,
        ///                  except the temporary audio file is also deleted. This makes the
        ///                  <see cref="AudioInfo.Path" /> property pointless in most cases,
        ///                  as it points to a file that no longer exists.
        /// </remarks>
        public AudioInfo GetAudioInformationInsideVideoAndClean()
        {
            string fileName = RandomPathGenerator.GenerateDirectoryNameV1();

            AudioKind ak = AudioManager.AutomatedExtractAudio(this, fileName)
                           ?? throw new InvalidOperationException("The video file seems to contain invalid or unsupported audio format.");

            var info = new AudioInfo($"{fileName}.{ak.ToString().ToLower()}");
            File.Delete($"{fileName}.{ak.ToString().ToLower()}");

            return info;
        }

        /// <summary>
        /// Concatenates this video with another one and saves it as <paramref name="outputVideoFile" />.
        /// </summary>
        /// <param name="video">Path to the video file that will be concatenated with this one.</param>
        /// <param name="outputVideoFile">Output video file name.</param>
        /// <remarks>
        ///     1. If the output video file exists, it will be overwritten.<br />
        ///     2. As tested, this method is <b>very</b> fast and uses little to no memory.
        /// </remarks>
        public void ConcatenateWith(Video video, string outputVideoFile) => ConcatenateWith(video.Path, outputVideoFile);

        /// <summary>
        /// Concatenates this video with another one and saves it as <paramref name="outputVideoFile" />.
        /// </summary>
        /// <param name="videoPath">Path to the video file that will be concatenated with this one.</param>
        /// <param name="outputVideoFile">Output video file name.</param>
        /// <remarks>
        ///     1. If the output video file exists, it will be overwritten.<br />
        ///     2. As tested, this method is <b>very</b> fast and uses little to no memory.
        /// </remarks>
        public void ConcatenateWith(string videoPath, string outputVideoFile)
        {
            string fileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1("txt");

            while (File.Exists(fileName))
            {
                fileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1("txt");
            }

            File.AppendAllText(fileName, $"file '{this.Path}'{Environment.NewLine}");
            File.AppendAllText(fileName, $"file '{videoPath}'{Environment.NewLine}");

            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -f concat -safe 0 -i \"{fileName}\" -c copy \"{outputVideoFile}\"");
        }
    }
}
