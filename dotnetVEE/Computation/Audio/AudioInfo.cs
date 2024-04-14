using dotnetVEE.Wrapper;

namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Represents information about an audio.
    /// </summary>
    public struct AudioInfo
    {
        /// <summary>
        /// Gets the duration of the audio.
        /// </summary>
        public TimeSpan Duration { get; private init; }

        /// <summary>
        /// Gets the duration of the audio in seconds (milliseconds are also included).
        /// </summary>
        public double DurationInSeconds { get; private init; }

        /// <summary>
        /// Gets the sample rate of an audio in Hz.
        /// </summary>
        public int SampleRate { get; private init; }

        /// <summary>
        /// Gets the path to the audio file.
        /// </summary>
        public string Path { get; private init; }

        /// <summary>
        /// Gets the volume of an audio. The default value is 100%.
        /// </summary>
        public Volume Volume { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioInfo" /> struct.
        /// </summary>
        /// <param name="fileName">Name of an audio file to process.</param>
        public AudioInfo(string fileName)
        {
            ThrowHelpers.FileNotFoundException(fileName);

            double duration = ProcessAudioDuration(fileName);
            Duration = TimeSpan.FromSeconds(duration);
            DurationInSeconds = duration;

            SampleRate = ProcessAudioSampleRate(fileName);

            Path = fileName;

            Volume = new Volume();
        }

        /// <summary>
        /// Returns duration of an audio in seconds.
        /// </summary>
        /// <param name="audioFile">Input audio file.</param>
        /// <returns>Duration of an audio in seconds.</returns>
        /// <exception cref="InvalidAudioFileException">Thrown when the audio file is invalid.</exception>
        public static double ProcessAudioDuration(string audioFile)
        {
            ThrowHelpers.FileNotFoundException(audioFile);

            string output = InvokeHelper.LaunchAndRedirectFFprobeOutput($"-i \"{audioFile}\" -show_entries format=duration -v quiet -of csv=\"p=0\"");

            bool parseSuccess = double.TryParse(output, out double result);
            if (!parseSuccess)
            {
                throw new InvalidAudioFileException();
            }

            return result;
        }

        /// <summary>
        /// Returns sample rate (in Hz) of an audio.
        /// </summary>
        /// <param name="audioFile">Input audio file.</param>
        /// <returns>Sample rate of an audio.</returns>
        /// <exception cref="InvalidAudioFileException">Thrown when the audio file is invalid.</exception>
        public static int ProcessAudioSampleRate(string audioFile)
        {
            ThrowHelpers.FileNotFoundException(audioFile);

            string output = InvokeHelper.LaunchAndRedirectFFprobeOutput($"-v error -select_streams a:0 -show_entries stream=sample_rate -of default=noprint_wrappers=1:nokey=1 \"{audioFile}\"");

            bool parseSuccess = int.TryParse(output, out int result);
            if (!parseSuccess)
            {
                throw new InvalidAudioFileException();
            }

            return result;
        }

        /// <summary>
        /// Implicitly represents <see cref="AudioInfo" /> from a string, where a string is
        /// the path to the audio file.
        /// </summary>
        /// <param name="audioFile">Input audio file.</param>
        public static implicit operator AudioInfo(string audioFile) => new AudioInfo(audioFile);
    }
}
