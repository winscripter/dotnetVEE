using dotnetVEE.Wrapper;
using System.Globalization;
using System.Numerics;

namespace dotnetVEE.Computation.Query
{
    /// <summary>
    /// Gets information about a video.
    /// </summary>
    public static class VideoDataQuery
    {
        /// <summary>
        /// Gets video dimensions, f.e. 1920x1080.
        /// </summary>
        /// <param name="video">Input video to get data for.</param>
        /// <returns>Resolution of the video.</returns>
        public static Vector2 GetVideoDimensions(string video)
        {
            InvokeHelper.EnsureFFprobeExists();

            var output = InvokeHelper.LaunchAndRedirectFFprobeOutput($"-v error -show_entries stream=width,height -of default=noprint_wrappers=1 \"{video}\"").Replace("\r\n", "\n");

            return new Vector2(
                Convert.ToSingle(output.Split('\n')[0].Split('=')[1], CultureInfo.InvariantCulture),
                Convert.ToSingle(output.Split('\n')[1].Split('=')[1], CultureInfo.InvariantCulture)
            );
        }

        /// <summary>
        /// Gets the amount of Frames Per Second (FPS) from a video.
        /// </summary>
        /// <param name="video">Input video file</param>
        /// <returns>Amount of FPS in a floating-point integer representation</returns>
        public static float GetFpsCount(string video)
        {
            InvokeHelper.EnsureFFprobeExists();
            InvokeHelper.EnsureVideoExists(video, nameof(video));

            var msg = $"-v error -select_streams v:0 -show_entries stream=r_frame_rate -of default=noprint_wrappers=1:nokey=1 \"{video}\"";

            var output = InvokeHelper.LaunchAndRedirectFFprobeOutput(msg).Trim().Split("/")[0];

            return float.Parse(output, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the amount of frames from a video.
        /// </summary>
        /// <param name="video">Video path.</param>
        /// <returns>Amount of frames in a video.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the video file is invalid.</exception>
        public static int GetFrameCount(string video)
        {
            InvokeHelper.EnsureFFmpegExists();

            string parameters = $"-v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0 \"{video}\"";

            var output = InvokeHelper.LaunchAndRedirectFFprobeOutput(parameters);

            try
            {
                return int.Parse(output);
            }
            catch
            {
                throw new InvalidOperationException("The video file is invalid");
            }
        }
    }
}
