using dotnetVEE.Abstractions;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Wrapper;

namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Provides tools for editing audios in a video.
    /// </summary>
    public static class AudioManager
    {
        /// <summary>
        /// Alters the volume of a video between the given two timestamps.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="timestamps">Start &amp; End timestamps for the effect.</param>
        /// <param name="volume">Volume of the audio.</param>
        /// <param name="output">Output video file.</param>
        public static void AlterVolume(Video vid, StartEndTimestamp timestamps, Volume volume, string output)
        {
            string startFormatted = timestamps.Start.TotalSeconds.ToString().Replace(",", ".");
            string endFormatted = timestamps.End.TotalSeconds.ToString().Replace(",", ".");

            string volumeFormatted = (volume.Value / 100).SafeStringify().Replace(",", ".");

            DebuggingHelper.Notice($"start: {startFormatted}, end: {endFormatted}, vol: {volumeFormatted}, initial vol: {volume.Value}");

            string args = $"-y -i \"{vid.Path}\" -af \"volume=enable='between(t,{startFormatted},{endFormatted})':volume={volumeFormatted}\" \"{output}\"";

            DebuggingHelper.Notice($"args are: {args}");
            InvokeHelper.LaunchAndWaitForFFmpeg(args);
        }

        /// <summary>
        /// Extracts audio from a video and saves it into an audio file.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="output">Output audio file, which is the audio extracted from a video.</param>
        /// <remarks>
        ///     <b>Note:</b> If the output audio file is an empty 0-byte audio file,
        ///                  it is a sign that the audio file is incompatible. If
        ///                  the video file has the audio in an AAC format and the
        ///                  destination file is audio.<b>mp3</b>, an empty audio.mp3
        ///                  audio file with nothing will be created. You need to use
        ///                  an audio format compatible with what is used in video.
        ///                  Most likely, it will be <c>AAC</c>, but you can also
        ///                  use the <see cref="AutomatedExtractAudio(Video, string)" />
        ///                  method to extract audio from a video with a correct
        ///                  format automatically (might be slower.)
        /// </remarks>
        public static void ExtractAudio(Video vid, string output)
            => InvokeHelper.LaunchAndWaitForFFmpeg($"-y -i \"{vid.Path}\" -vn -acodec copy \"{output}\"");

        /// <summary>
        /// Automatically extracts audio from a video to a compatible output audio.
        /// </summary>
        /// <param name="vid">Input video.</param>
        /// <param name="output">Output audio file name.</param>
        /// <returns>Type of an output audio file, or <see langword="NULL" /> if no audio type was compatible.</returns>
        /// <remarks>
        ///     <b>Note #1:</b> Parameter <paramref name="output" /> must not end
        ///                     with a file extension. While it <b>can</b>, dotnetVEE
        ///                     will automatically append a correct file extension. So,
        ///                     if <paramref name="output" /> is <b>audio.mp3</b> and
        ///                     the correct audio format is <b>AAC</b>, output file
        ///                     will be saved as <b>audio.mp3.aac</b>. Even if the
        ///                     correct audio format is <b>MP3</b>, output file name
        ///                     will still be saved as <b>audio.mp3.mp3</b>. It is
        ///                     recommended not to use any extension, like so:
        ///                     <c>audio</c>. <br /><br />
        ///                     
        ///     <b>Note #2:</b> This method has degraded performance compared
        ///                     to <see cref="ExtractAudio(Video, string)" />,
        ///                     unless <b>AAC</b> is the correct audio format. <br /><br />
        ///                     
        ///     <b>Note #3:</b> This method returns an <see cref="AudioKind" /> enum.
        ///                     To get the file name of the output audio file, use:
        ///                     <code>var kind = AudioManager.AutomatedExtractAudio(vid, output);
        /// Console.WriteLine(output + kind?.ToString()); // audio.aac</code>
        /// </remarks>
        public static AudioKind? AutomatedExtractAudio(Video vid, string output)
        {
            foreach (AudioKind ak in (AudioKind[])Enum.GetValues(typeof(AudioKind)))
            {
                string outputFileName = $"{output}.{ak.ToString().ToLower()}";
                DebuggingHelper.Notice(outputFileName);
                ExtractAudio(vid, outputFileName);

                if (IsEmpty(outputFileName))
                {
                    File.Delete(outputFileName);

                    continue;
                }
                else
                {
                    return ak;
                }
            }

            return null;

            static bool IsEmpty(string fileName) => new FileInfo(fileName).Length == 0;
        }
    }
}
