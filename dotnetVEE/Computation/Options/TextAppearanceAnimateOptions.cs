using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.UtilitySpecific;
using dotnetVEE.Computation.Utils;
using System.Runtime.CompilerServices;

namespace dotnetVEE.Computation.Options
{
    /// <summary>
    /// Options for animating text appearing and vanishing. Used by the <see cref="TextAppearanceUtility" /> utility.
    /// </summary>
    public struct TextAppearanceAnimateOptions
    {
        /// <summary>
        /// Text visibility options.
        /// </summary>
        public TextComputationOptions ViewOptions { get; init; }

        /// <summary>
        /// Start &amp; End timestamps for starting and ending the animation.
        /// </summary>
        public StartEndTimestamp Timestamps { get; init; }

        /// <summary>
        /// Amount of time for how long the <b>appear</b> part lasts for.
        /// </summary>
        public TimeSpan StartDuration { get; init; }
        
        /// <summary>
        /// Amount of time for how long the text stays still.
        /// </summary>
        public TimeSpan AppearDuration { get; init; }

        /// <summary>
        /// Amount of time for how long the <b>vanish</b> part lasts for.
        /// </summary>
        public TimeSpan EndDuration { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAppearanceAnimateOptions" /> struct.
        /// </summary>
        /// <param name="viewOptions">Text visibility options.</param>
        /// <param name="timestamps">Start &amp; End timestamps for starting and ending the animation.</param>
        /// <param name="startDuration">Amount of time for how long the <b>appear</b> part lasts for.</param>
        /// <param name="appearDuration">Amount of time for how long the text stays still.</param>
        /// <param name="endDuration">Amount of time for how long the <b>vanish</b> part lasts for.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextAppearanceAnimateOptions(
            TextComputationOptions viewOptions,
            StartEndTimestamp timestamps,
            TimeSpan startDuration,
            TimeSpan appearDuration,
            TimeSpan endDuration)
        {
            ViewOptions = viewOptions;
            Timestamps = timestamps;
            StartDuration = startDuration;
            AppearDuration = appearDuration;
            EndDuration = endDuration;
        }
    }
}
