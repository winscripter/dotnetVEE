using SixLabors.ImageSharp;

namespace dotnetVEE.Computation.Options.Common
{
    /// <summary>
    /// Base options for most motion options.
    /// </summary>
    /// <param name="Start">Start point of the object.</param>
    /// <param name="End">End point of the object.</param>
    /// <param name="StartTimestamp">Start timestamp of the object, when it starts to appear. Duration will be adjusted accordingly.</param>
    /// /// <param name="EndTimestamp">End timestamp of the object, when it stops. Duration will be adjusted accordingly.</param>
    public record BaseMotionOptions(
        Point Start,
        Point End,
        TimeSpan StartTimestamp,
        TimeSpan EndTimestamp
    );
}
