using dotnetVEE.Abstractions;

namespace dotnetVEE.Computation.Base
{
    /// <summary>
    /// Specifies an object that can move at a specific
    /// speed from one x/y coordinate to another.
    /// </summary>
    public abstract class Motion
    {
        /// <summary>
        /// Speed of the moving object.
        /// </summary>
        protected MotionSpeed Speed { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Motion" /> class.
        /// </summary>
        /// <param name="length">Speed of the motion.</param>
        protected Motion(MotionSpeed length)
        {
            Speed = length;
        }
    }
}
