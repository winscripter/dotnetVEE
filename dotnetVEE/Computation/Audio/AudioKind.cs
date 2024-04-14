namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Represents type of an audio file.
    /// </summary>
    public enum AudioKind : byte
    {
        /// <summary>
        /// AAC Audio type (*.aac).
        /// </summary>
        Aac,

        /// <summary>
        /// MP3 Audio type (*.mp3).
        /// </summary>
        Mp3,

        /// <summary>
        /// OGG Audio type (*.ogg).
        /// </summary>
        Ogg,

        /// <summary>
        /// Wave Audio type (*.wav).
        /// </summary>
        Wav,

        /// <summary>
        /// WebM Audio type (*.webm).
        /// </summary>
        Webm,

        /// <summary>
        /// WMA Audio type (*.wma).
        /// </summary>
        Wma,

        /// <summary>
        /// M4a Audio type (*.m4a).
        /// </summary>
        M4a
    }
}
