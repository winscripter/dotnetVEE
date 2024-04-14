namespace dotnetVEE.Abstractions.UtilitySpecific.Exceptions
{
    /// <summary>
    /// Represents an exception thrown from a utility.
    /// </summary>
    public class UtilityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UtilityException" /> class.
        /// </summary>
        public UtilityException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UtilityException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UtilityException(string message)
            : base(message)
        {
        }
    }
}
