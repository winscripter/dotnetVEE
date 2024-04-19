namespace dotnetVEE.Private.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Makes the first character in the string uppercase.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>A new string with the first letter being uppercase.</returns>
        /// <exception cref="ArgumentException">Thrown when the string is empty.</exception>
        public static string Capitalize(this string input) =>
            input switch
            {
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
    }
}
