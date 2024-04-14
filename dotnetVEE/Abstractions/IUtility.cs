using System.Collections.ObjectModel;

namespace dotnetVEE.Abstractions
{
    /// <summary>
    /// Represents a utility - that is, a task that makes changes to a video.
    /// </summary>
    public interface IUtility
    {
        /// <summary>
        /// Runs this utility.
        /// </summary>
        /// <param name="vid">The video context.</param>
        /// <param name="progress">The constantly updating progress of this utility.</param>
        void Run(Video vid, ref ObservableCollection<float> progress);
    }
}
