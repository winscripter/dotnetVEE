namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// An exception thrown when an audio file is not valid.
    /// </summary>
    public class InvalidAudioFileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAudioFileException" /> class.
        /// </summary>
        public InvalidAudioFileException()
            : this("The audio file is invalid")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAudioFileException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidAudioFileException(string message)
            : base(message)
        {
        }
    }
}
