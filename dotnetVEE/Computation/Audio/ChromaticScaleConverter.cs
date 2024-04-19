namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Provides various methods to convert data related to chromatic scale.
    /// </summary>
    public static class ChromaticScaleConverter
    {
        /// <summary>
        /// Converts Chromatic Scale and Octave value to semitones. To do
        /// the opposite, use the <see cref="FromSemitones(int)" /> method.
        /// </summary>
        /// <param name="scale">The Chromatic Scale value.</param>
        /// <param name="octave">The Octave value.</param>
        /// <returns>Number of Semitones.</returns>
        public static int ToSemitones(ChromaticScale scale, int octave = 0) => octave * 12 + (int)scale;

        /// <summary>
        /// Converts Chromatic Scale and Octave value to semitones. To do
        /// the opposite, use the <see cref="FromSemitones(int)" /> method.
        /// </summary>
        /// <param name="group">The input Chromatic Scale and Octave value.</param>
        /// <returns>Number of Semitones.</returns>
        public static int ToSemitones(ChromaticScaleOctaveGroup group) => ToSemitones(group.ChromaticScale, group.Octave);

        /// <summary>
        /// Converts semitones into Chromatic Scale and Octave. To do the opposite,
        /// use the <see cref="ToSemitones(ChromaticScale, int)" /> method.
        /// </summary>
        /// <param name="semitone">Amount of semitones to convert.</param>
        /// <returns>An instance of <see cref="ChromaticScaleOctaveGroup" />, holding chromatic scale and octave.</returns>
        public static ChromaticScaleOctaveGroup FromSemitones(int semitone)
        {
            ChromaticScale note = (ChromaticScale)(semitone % 12);
            int octave = semitone / 12;

            return new(note, octave);
        }
    }
}
