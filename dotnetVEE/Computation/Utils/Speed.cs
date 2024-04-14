using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// A utility that speeds up a part of video.
    /// </summary>
    /// <remarks>
    /// The audio will be removed from the video. Please make sure
    /// to extract the audio first using the <see cref="Audio.AudioManager.AutomatedExtractAudio(Video, string)" />
    /// method and then add the audio to the video back. You may
    /// also want to speed up the extracted audio to yield similar
    /// effect.
    /// </remarks>
    public class Speed : IUtility
    {
        private readonly float _newSpeed;
        private readonly StartEndTimestamp _timestamps;

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed" /> utility class.
        /// </summary>
        /// <param name="newSpeed">The new speed for the video (100% is considered default.)</param>
        /// <param name="timestamps">Start &amp; End timestamps where the speed will change.</param>
        public Speed(float newSpeed, StartEndTimestamp timestamps)
        {
            _newSpeed = newSpeed;
            _timestamps = timestamps;
        }

        /// <summary>
        /// Path to the output video file, or <see langword="NULL" /> if it's unset.
        /// </summary>
        public string? OutputVideoFilePath { get; private set; } = null;

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0F);

            string start = _timestamps.Start.Stringify();
            string end = _timestamps.End.Stringify();
            string pts = (1 / (_newSpeed / 100)).ToString().Replace(",", ".");

            string path = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4");

            string command = $"-i \"{vid.Path}\" -filter_complex \"[0:v]trim=start={start}:end={end},setpts={pts}*PTS[v1];[0:v]trim=start={start},setpts=PTS-STARTPTS[v2];[v1][v2]concat=n=2:v=1:a=0[out]\" -map \"[out]\" \"{path}\"";

            //DebuggingHelper.Notice(start);
            //DebuggingHelper.Notice(end);
            //DebuggingHelper.Notice(pts);
            //DebuggingHelper.Notice(path);
            //DebuggingHelper.Notice(command);

            InvokeHelper.LaunchAndWaitForFFmpeg(command);

            OutputVideoFilePath = path;

            progress.Add(100F);
        }
    }
}
