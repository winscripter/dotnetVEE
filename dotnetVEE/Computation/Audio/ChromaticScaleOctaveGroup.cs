namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Groups <see cref="Audio.ChromaticScale" /> and octave value.
    /// </summary>
    /// <param name="ChromaticScale">The Chromatic Scale.</param>
    /// <param name="Octave">The Octave value.</param>
    public record struct ChromaticScaleOctaveGroup(ChromaticScale ChromaticScale, int Octave = 0);
}
