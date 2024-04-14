namespace dotnetVEE.Abstractions.FileGeneration
{
    /// <summary>
    /// Specifies names for different generated files by some utility.
    /// </summary>
    /// <param name="FramesDirectoryName">Name of the generated frames directory (if present).</param>
    /// <param name="FramesDirectoryFullPath">Full path to the generated frames directory (if present).</param>
    /// <param name="VideoFileName">Name of the generated video file (if present).</param>
    /// <param name="VideoFileFullPath">Full path to the generated video file (if present).</param>
    public record struct GeneratedFileNames(
        string? FramesDirectoryName,
        string? FramesDirectoryFullPath,
        string? VideoFileName,
        string? VideoFileFullPath
    );
}
