namespace dotnetVEE.Computation
{
    /// <summary>
    /// Represents a planar Chroma Subsampling Scheme.
    /// See <see href="https://en.wikipedia.org/wiki/Chroma_subsampling" />.
    /// </summary>
    public enum ChromaSubsamplingScheme : byte
    {
        /// <summary>
        /// YUV411p FFmpeg Pixel Format (least quality).
        /// </summary>
        Yuv411p,

        /// <summary>
        /// YUV420p FFmpeg Pixel Format (average quality).
        /// </summary>
        Yuv420p,

        /// <summary>
        /// YUV422p FFmpeg Pixel Format (higher than average quality).
        /// </summary>
        Yuv422p,

        /// <summary>
        /// YUV444p FFmpeg Pixel Format (highest quality).
        /// </summary>
        Yuv444p
    }
}
