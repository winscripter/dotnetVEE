using dotnetVEE.Private.Extensions;
using dotnetVEE.Wrapper;

namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Provides methods to manipulate audio files directly.
    /// </summary>
    /// <remarks>
    /// <see cref="AudioFx" /> only applies to audio files, like MP3, WAV, OGG, etc.
    /// To modify audio playing inside a video, you need to:<br />
    /// <list type="bullet">
    ///     <item>1. Extract audio</item>
    ///     <item>2. Mute that part</item>
    ///     <item>3. Manipulate the extracted audio file with this class</item>
    ///     <item>4. Add it back</item>
    /// </list>
    /// </remarks>
    public static class AudioFx
    {
        /// <summary>
        /// Gets the file extension of the audio file.
        /// </summary>
        /// <param name="audioFile">Input audio file.</param>
        /// <returns>The extension of the audio file represented as a string.</returns>
        /// <exception cref="InvalidAudioFileException">Thrown when the file doesn't seem to have an extension.</exception>
        public static string GetAudioExtension(string audioFile)
            => audioFile.Contains('.') ? audioFile.Split('.').Last() : throw new InvalidAudioFileException();

        /// <summary>
        /// Converts the file extension of the video to an <see cref="AudioKind" /> enum.
        /// </summary>
        /// <param name="extension">File extension of the audio. Period characters will be automatically deleted.</param>
        /// <returns>File extension represented in <see cref="AudioKind" />, or <see langword="NULL" /> if it's unsupported.</returns>
        /// <exception cref="ArgumentException">Thrown when the extension contains different characters than [A-Z 0-9].</exception>
        public static AudioKind? ConvertExtensionToKind(string extension)
        {
            extension = extension.Replace(".", string.Empty);

            AudioKind[] enumValues = Enum.GetValues<AudioKind>();
            IEnumerable<AudioKind> matchingItems = enumValues.Where(e => e.ToString().ToLower() == extension.ToLower());

            return matchingItems.Count() == 0 ? null : matchingItems.First();
        }

        /// <summary>
        /// Concatenates the first audio file, <paramref name="destination" />,
        /// with <paramref name="source" /> and saves it as <paramref name="output" />.
        /// </summary>
        /// <param name="destination">Destination audio file: where source will be concatenated.</param>
        /// <param name="source">Source audio file: audio file to concatenate to destination.</param>
        /// <param name="output">Output audio file name.</param>
        /// <remarks>
        ///     If the output file name exists, it will be overwritten.
        /// </remarks>
        public static void Concatenate(string destination, string source, string output)
        {
            ThrowHelpers.FileNotFoundException(destination);
            ThrowHelpers.FileNotFoundException(source);

            const string fxListTempFile = "AudioFx.Concatenate.Lists.txt";

            if (File.Exists(fxListTempFile))
            {
                throw new IOException($"File \"{fxListTempFile}\" should not exist.");
            }

            if (destination.Contains('\'') || source.Contains('\''))
            {
                throw new IOException("Destination and source files should not contain single quotes in the file name.");
            }

            File.AppendAllText(fxListTempFile, $"file '{destination}'{Environment.NewLine}");
            File.AppendAllText(fxListTempFile, $"file '{source}'");

            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -f concat -safe 0 -i \"{fxListTempFile}\" -c copy \"{output}\"");

            File.Delete(fxListTempFile);
        }

        /// <summary>
        /// Concatenates the first audio file, <paramref name="destination" />,
        /// with <paramref name="source" /> and saves it as <paramref name="output" />.
        /// </summary>
        /// <param name="destination">Destination audio file: where source will be concatenated.</param>
        /// <param name="source">Source audio file: audio file to concatenate to destination.</param>
        /// <param name="output">Output audio file name.</param>
        /// <remarks>
        ///     If the output file name exists, it will be overwritten.
        /// </remarks>
        public static void Concatenate(AudioInfo destination, string source, string output) => Concatenate(destination.Path, source, output);

        /// <summary>
        /// Concatenates the first audio file, <paramref name="destination" />,
        /// with <paramref name="source" /> and saves it as <paramref name="output" />.
        /// </summary>
        /// <param name="destination">Destination audio file: where source will be concatenated.</param>
        /// <param name="source">Source audio file: audio file to concatenate to destination.</param>
        /// <param name="output">Output audio file name.</param>
        /// <remarks>
        ///     If the output file name exists, it will be overwritten.
        /// </remarks>
        public static void Concatenate(string destination, AudioInfo source, string output) => Concatenate(destination, source.Path, output);

        /// <summary>
        /// Concatenates the first audio file, <paramref name="destination" />,
        /// with <paramref name="source" /> and saves it as <paramref name="output" />.
        /// </summary>
        /// <param name="destination">Destination audio file: where source will be concatenated.</param>
        /// <param name="source">Source audio file: audio file to concatenate to destination.</param>
        /// <param name="output">Output audio file name.</param>
        /// <remarks>
        ///     If the output file name exists, it will be overwritten.
        /// </remarks>
        public static void Concatenate(AudioInfo destination, AudioInfo source, string output) => Concatenate(destination.Path, source, output);

        /// <summary>
        /// Alters the tempo of an audio. That is, speeding up the
        /// audio without affecting the pitch.
        /// </summary>
        /// <param name="source">The source audio file.</param>
        /// <param name="by">Percentage of the tempo effect. Default is 100%. For instance, to speed up the audio by 1.5x, use the value 150.0.</param>
        /// <param name="output">The output audio file.</param>
        /// <remarks>
        ///     The output audio file will be overwritten if it exists.
        /// </remarks>
        public static void Tempo(string source, float by, string output)
        {
            ThrowHelpers.FileNotFoundException(source);

            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -i \"{source}\" -filter:a \"rubberband=tempo={(by / 100F).SafeStringify()}\" \"{output}\"");
        }

        /// <summary>
        /// Alters the tempo of an audio. That is, speeding up the
        /// audio without affecting the pitch.
        /// </summary>
        /// <param name="source">The source audio file.</param>
        /// <param name="by">Percentage of the tempo effect. Default is 100%. For instance, to speed up the audio by 1.5x, use the value 150.0.</param>
        /// <param name="output">The output audio file.</param>
        /// <remarks>
        ///     The output audio file will be overwritten if it exists.
        /// </remarks>
        public static void Tempo(AudioInfo source, float by, string output) => Tempo(source.Path, by, output);

        /// <summary>
        /// Alters the tempo of an audio. That is, speeding up the
        /// audio without affecting the pitch.
        /// </summary>
        /// <param name="source">The source audio file.</param>
        /// <param name="by">Percentage of the tempo effect. Default is 100%. For instance, to speed up the audio by 1.5x, use the value 150.0.</param>
        /// <param name="output">The output audio file.</param>
        /// <remarks>
        ///     The output audio file will be overwritten if it exists.
        /// </remarks>
        public static void Tempo(string source, float by, AudioInfo output) => Tempo(source, by, output.Path);

        /// <summary>
        /// Alters the tempo of an audio. That is, speeding up the
        /// audio without affecting the pitch.
        /// </summary>
        /// <param name="source">The source audio file.</param>
        /// <param name="by">Percentage of the tempo effect. Default is 100%. For instance, to speed up the audio by 1.5x, use the value 150.0.</param>
        /// <param name="output">The output audio file.</param>
        /// <remarks>
        ///     The output audio file will be overwritten if it exists.
        /// </remarks>
        public static void Tempo(AudioInfo source, float by, AudioInfo output) => Tempo(source.Path, by, output.Path);

        /// <summary>
        /// Adds a pitch effect to an audio file.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="pitch">Pitch value. Default is 1.0. Lesser values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        /// <remarks>
        ///     This alters the pitch value <i>by a multiplicative value</i>. For instance, passing
        ///     <c>3.0</c> as a pitch value could result in a very severe pitch effect, to the point
        ///     where it's hard to hear the audio properly. To apply the pitch effect by semitones,
        ///     please use the <see cref="PitchBySemitones(string, int, string)" /> method instead.
        /// </remarks>
        public static void Pitch(string source, float pitch, string output)
        {
            ThrowHelpers.FileNotFoundException(source);

            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -i \"{source}\" -filter:a \"rubberband=pitch={pitch.SafeStringify()}\" \"{output}\"");
        }

        /// <summary>
        /// Adds a pitch effect to an audio file.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="pitch">Pitch value. Default is 1.0. Lesser values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        /// <remarks>
        ///     This alters the pitch value <i>by a multiplicative value</i>. For instance, passing
        ///     <c>3.0</c> as a pitch value could result in a very severe pitch effect, to the point
        ///     where it's hard to hear the audio properly. To apply the pitch effect by semitones,
        ///     please use the <see cref="PitchBySemitones(string, int, string)" /> method instead.
        /// </remarks>
        public static void Pitch(AudioInfo source, float pitch, string output) => Pitch(source.Path, pitch, output);

        /// <summary>
        /// Adds a pitch effect to an audio file.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="pitch">Pitch value. Default is 1.0. Lesser values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        /// <remarks>
        ///     This alters the pitch value <i>by a multiplicative value</i>. For instance, passing
        ///     <c>3.0</c> as a pitch value could result in a very severe pitch effect, to the point
        ///     where it's hard to hear the audio properly. To apply the pitch effect by semitones,
        ///     please use the <see cref="PitchBySemitones(string, int, string)" /> method instead.
        /// </remarks>
        public static void Pitch(string source, float pitch, AudioInfo output) => Pitch(source, pitch, output.Path);

        /// <summary>
        /// Adds a pitch effect to an audio file.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="pitch">Pitch value. Default is 1.0. Lesser values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        /// <remarks>
        ///     This alters the pitch value <i>by a multiplicative value</i>. For instance, passing
        ///     <c>3.0</c> as a pitch value could result in a very severe pitch effect, to the point
        ///     where it's hard to hear the audio properly. To apply the pitch effect by semitones,
        ///     please use the <see cref="PitchBySemitones(string, int, string)" /> method instead.
        /// </remarks>
        public static void Pitch(AudioInfo source, float pitch, AudioInfo output) => Pitch(source.Path, pitch, output.Path);

        /// <summary>
        /// Adds a pitch effect to an audio file with semitones, as opposed to
        /// the <see cref="Pitch(string, float, string)" /> method that uses a
        /// multiplicative value instead of additive.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="semitones">Pitch value. Default is 0. Negative values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        public static void PitchBySemitones(string source, int semitones, string output)
        {
            ThrowHelpers.FileNotFoundException(source);

            string value = GetMultiplicativeValueFromSemitones(semitones).SafeStringify();
            InvokeHelper.LaunchAndWaitForFFmpeg($"-y -i \"{source}\" -filter:a \"rubberband=pitch={value}\" \"{output}\"");

            static double GetMultiplicativeValueFromSemitones(int semitones) => Math.Pow(1.059463094352953D, semitones);
        }

        /// <summary>
        /// Adds a pitch effect to an audio file with semitones, as opposed to
        /// the <see cref="Pitch(string, float, string)" /> method that uses a
        /// multiplicative value instead of additive.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="semitones">Pitch value. Default is 0. Negative values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        public static void PitchBySemitones(AudioInfo source, int semitones, string output)
            => PitchBySemitones(source.Path, semitones, output);

        /// <summary>
        /// Adds a pitch effect to an audio file with semitones, as opposed to
        /// the <see cref="Pitch(string, float, string)" /> method that uses a
        /// multiplicative value instead of additive.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="semitones">Pitch value. Default is 0. Negative values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        public static void PitchBySemitones(string source, int semitones, AudioInfo output)
            => PitchBySemitones(source, semitones, output.Path);

        /// <summary>
        /// Adds a pitch effect to an audio file with semitones, as opposed to
        /// the <see cref="Pitch(string, float, string)" /> method that uses a
        /// multiplicative value instead of additive.
        /// </summary>
        /// <param name="source">Input audio file.</param>
        /// <param name="semitones">Pitch value. Default is 0. Negative values will result in a darker sound.</param>
        /// <param name="output">Output audio file. This will be overwritten if it exists.</param>
        public static void PitchBySemitones(AudioInfo source, int semitones, AudioInfo output)
            => PitchBySemitones(source.Path, semitones, output.Path);
    }
}
