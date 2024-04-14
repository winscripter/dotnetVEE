using dotnetVEE.Abstractions;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// Extracts frames starting and ending with given frame indexes
    /// from a video.
    /// </summary>
    /// <remarks>
    /// The Frame Index starts with 0. For example, to refer to the 37th frame specify 36 instead.
    /// This is a simple automation process that keeps calling <see cref="ExtractFrameAtIndex" />
    /// multiple times for the sake of performance.
    /// </remarks>
    public class ExtractFramesWithIndex : IUtility
    {
        private readonly int _startIndex;
        private readonly int _endIndex;
        private readonly string _outputDir;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="startIndex">The start index where the next frames start to extract.</param>
        /// <param name="endIndex">The end index being the last frame to be extracted.</param>
        /// <param name="outputDir">Output directory containing output frames.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="endIndex" /> is less than or equal to <paramref name="startIndex" />.</exception>
        public ExtractFramesWithIndex(int startIndex, int endIndex, string outputDir)
        {
            if (endIndex <= startIndex)
            {
                throw new ArgumentException("Parameter must be greater than the start index", nameof(endIndex));
            }

            _startIndex = startIndex;
            _endIndex = endIndex;
            _outputDir = outputDir;
        }

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);
            float percentageBy = 100F / (float)(_endIndex - _startIndex);

            float thisPercentage = 0.0F;
            int globalFrameIndex = 0;

            for (int i = _startIndex; i <= _endIndex; i++)
            {
                if (!Directory.Exists(_outputDir))
                {
                    Directory.CreateDirectory(_outputDir);
                }

                var utility = (IUtility)new ExtractFrameAtIndex(i, $"{_outputDir}/frame{(globalFrameIndex++).ToString().PadLeft(Constants.TrailingPadNumberCount, '0')}.png");
                var empty = new ObservableCollection<float>();
                utility.Run(vid, ref empty);

                thisPercentage += percentageBy;
                progress.Add(thisPercentage);
            }

            progress.Add(100.0F);
        }
    }
}
