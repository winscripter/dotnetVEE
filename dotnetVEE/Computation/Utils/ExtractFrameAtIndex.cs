using dotnetVEE.Abstractions;
using System.Collections.ObjectModel;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// A task to extract a frame at a given index.
    /// </summary>
    public class ExtractFrameAtIndex : IUtility
    {
        private readonly int _index;
        private readonly string _fileName;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="index">Frame index.</param>
        /// <param name="fileName">Name of the output frame file.</param>
        public ExtractFrameAtIndex(int index, string fileName)
        {
            _index = index;
            _fileName = fileName;
        }

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);
            vid.ExtractFrameAt(_index, _fileName);
            progress.Add(100.0F);
        }
    }
}
