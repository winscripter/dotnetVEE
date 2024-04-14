namespace dotnetVEE.Abstractions.FileGeneration
{
    /// <summary>
    /// An enum consisting of options when it comes to deleting generated
    /// files by an utility.
    /// </summary>
    public enum DeleteGeneratedFiles : byte
    {
        /// <summary>
        /// Do not delete any generated files - leave them as is.
        /// </summary>
        None,

        /// <summary>
        /// Delete just the generated video - any changes will be made to
        /// the original video.
        /// </summary>
        VideoOnly,

        /// <summary>
        /// Delete just the frames directory.
        /// </summary>
        FramesDirectoryOnly,

        /// <summary>
        /// Delete both the generated video and the frames directory - any changes
        /// will be made to the original video.
        /// </summary>
        Both
    }
}
