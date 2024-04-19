namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Provides Chromatic Scale values and their corresponding
    /// semitones in the octave 0. For instance, <see cref="ChromaticScale.C" />
    /// corresponds to 0 semitones, <see cref="ChromaticScale.CSharpDFlat" />
    /// to 1 semitone, and so on.<br /><br />
    /// 
    /// See <see href="https://en.wikipedia.org/wiki/Chromatic_scale" />.
    /// </summary>
    public enum ChromaticScale : byte
    {
        /// <summary>
        /// C Chromatic Scale.
        /// </summary>
        C,

        /// <summary>
        /// C♯/D♭ Chromatic Scale.
        /// </summary>
        /// <remarks>
        /// Not to be confused with the C# programming language.
        /// </remarks>
        CSharpDFlat,

        /// <summary>
        /// D Chromatic Scale.
        /// </summary>
        D,

        /// <summary>
        /// D♯/E♭ Chromatic Scale.
        /// </summary>
        DSharpEFlat,

        /// <summary>
        /// E Chromatic Scale.
        /// </summary>
        E,

        /// <summary>
        /// F Chromatic Scale.
        /// </summary>
        F,

        /// <summary>
        /// F♯/G♭ Chromatic Scale.
        /// </summary>
        /// <remarks>
        /// Not to be confused with the F# programming language.
        /// </remarks>
        FSharpGFlat,

        /// <summary>
        /// G Chromatic Scale.
        /// </summary>
        G,

        /// <summary>
        /// G♯/A♭ Chromatic Scale.
        /// </summary>
        GSharpAFlat,

        /// <summary>
        /// A Chromatic Scale.
        /// </summary>
        A,

        /// <summary>
        /// A♯/B♭ Chromatic Scale.
        /// </summary>
        ASharpBFlat,

        /// <summary>
        /// B Chromatic Scale.
        /// </summary>
        B,
    }
}
