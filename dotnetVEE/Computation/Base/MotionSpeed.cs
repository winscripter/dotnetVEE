namespace dotnetVEE.Computation.Base
{
    /// <summary>
    /// Represents the speed of the motion.
    /// </summary>
    public class MotionSpeed
    {
        /// <summary>
        /// Gets the <see cref="MotionSpeed" /> template with the speed rate being 0,
        /// e.g. the object will stay still.
        /// </summary>
        public static readonly MotionSpeed Frozen = 0;

        /// <summary>
        /// Gets the <see cref="MotionSpeed" /> template with the speed rate being 30,
        /// e.g. the average default speed.
        /// </summary>
        public static readonly MotionSpeed Default = 30;

        /// <summary>
        /// Gets the speed rate of this <see cref="MotionSpeed" /> instance.
        /// </summary>
        public float SpeedRate { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionSpeed" /> class.
        /// </summary>
        /// <param name="speedRate">Speed rate of the moving object.</param>
        /// <exception cref="ArgumentException">Thrown when the given speed rate is invalid.</exception>
        public MotionSpeed(float speedRate)
        {
            if (speedRate is < 0 or > 10000.0F)
            {
                throw new ArgumentException($"Motion speed can't be greater than 10000% or negative", nameof(speedRate));
            }

            this.SpeedRate = speedRate;
        }

        /// <summary>
        /// Shorthand for the constructor <see cref="MotionSpeed.MotionSpeed(float)" />.
        /// </summary>
        /// <param name="speed">Speed rate of the moving object.</param>
        public static implicit operator MotionSpeed(float speed) => new MotionSpeed(speed);
    }
}
