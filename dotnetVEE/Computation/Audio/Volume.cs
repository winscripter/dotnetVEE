using System.Data.Common;

namespace dotnetVEE.Computation.Audio
{
    /// <summary>
    /// Represents volume of an audio.
    /// </summary>
    public struct Volume
    {
        /// <summary>
        /// Default volume percentage, which is 100%.
        /// </summary>
        public const float Default = 100F;

        /// <summary>
        /// Amount of percentages that will be increased by the
        /// <see cref="DynamicIncrease" /> method.
        /// </summary>
        public float IncreaseBy = 2F;

        /// <summary>
        /// Amount of percentages that will be decreased by the
        /// <see cref="DynamicDecrease" /> method.
        /// </summary>
        public float DecreaseBy = 2F;

        private float volume;

        /// <summary>
        /// Initializes a new instance of the <see cref="Volume" /> struct.
        /// </summary>
        public Volume()
            : this(100F)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Volume" /> struct.
        /// </summary>
        /// <param name="volume">Volume (in percents.)</param>
        public Volume(float volume)
        {
            if (volume is < 0F) throw new ArgumentException("Volume can't be less than 0%", nameof(volume));

            this.volume = volume;
        }

        /// <summary>
        /// Represents amount of volume in this instance of <see cref="Volume" /> in percents.
        /// </summary>
        public float Value
        {
            get => volume;
            set => volume = value;
        }

        /// <summary>
        /// Increases the volume by 1 percent.
        /// </summary>
        public void Increase() => Value++;

        /// <summary>
        /// Decreases the volume by 1 percent.
        /// </summary>
        /// <remarks>
        ///     If the result is negative, dotnetVEE will fallback to 0%.
        /// </remarks>
        public void Decrease()
        {
            if (Value > 0)
            {
                Value--;
            }
        }

        /// <summary>
        /// Increases the volume by <c>x</c> percent(s), where <c>x</c> is the value of <see cref="IncreaseBy" />.
        /// </summary>
        public void DynamicIncrease() => Value += IncreaseBy;

        /// <summary>
        /// Decreases the volume by <c>x</c> percent(s), where <c>x</c> is the value of <see cref="DecreaseBy" />.
        /// </summary>
        /// <remarks>
        ///     If the result is negative, dotnetVEE will fallback to 0%.
        /// </remarks>
        public void DynamicDecrease()
        {
            if ((Value - DecreaseBy) > 0)
            {
                Value -= DecreaseBy;
            }
            else
            {
                Value = 0;
            }
        }

        /// <summary>
        /// Implicitly represents <see cref="float" /> as <see cref="Volume" />.
        /// </summary>
        /// <param name="volume">Volume (in percents.)</param>
        public static implicit operator Volume(float volume) => new Volume(volume);

        /// <summary>
        /// Casts <see cref="Volume" /> to <see cref="float" />, where the latter is
        /// the amount of percents in the instance.
        /// </summary>
        /// <param name="vol">Input instance of <see cref="Volume" />.</param>
        public static explicit operator float(Volume vol) => vol.Value;
    }
}
