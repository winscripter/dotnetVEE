namespace dotnetVEE.Abstractions.Exceptions
{
    /// <summary>
    /// An error, representing an unsupported video format.
    /// </summary>
    public class UnsupportedVideoFormatException : Exception
    {
        /// <summary>
        /// Name of the unsupported video format.
        /// </summary>
        public string? FormatName { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedVideoFormatException" /> class.
        /// </summary>
        public UnsupportedVideoFormatException()
            : this("The video format is not supported by dotnetVEE.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedVideoFormatException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UnsupportedVideoFormatException(string message)
            : base(message)
        {
            FormatName = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedVideoFormatException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="formatName">Format name.</param>
        public UnsupportedVideoFormatException(string message, string formatName)
            : base($"{message}{Environment.NewLine}Format name: \"{formatName}\"")
        {
            FormatName = formatName;
        }
    }
}
