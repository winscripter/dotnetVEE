namespace dotnetVEE.Abstractions.FileGeneration
{
    /// <summary>
    /// Specifies a utility that can generate files.
    /// </summary>
    public interface IGeneratesFiles
    {
        /// <summary>
        /// File names that the utility generated.
        /// </summary>
        GeneratedFileNames? FileNames { get; set; }
    }
}
