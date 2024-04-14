namespace dotnetVEE.Abstractions
{
    /// <summary>
    /// Represents a group of two timestamps to represent start-and-end.
    /// </summary>
    /// <param name="Start">Start timestamp.</param>
    /// <param name="End">End timestamp.</param>
    public record StartEndTimestamp(
        TimeSpan Start,
        TimeSpan End);
}
