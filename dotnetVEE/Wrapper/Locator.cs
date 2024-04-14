namespace dotnetVEE.Wrapper
{
    /// <summary>
    /// Locates FFmpeg and FFprobe
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Directory where FFmpeg (and FFprobe) are located.
        /// </summary>
        public string ToolDirectory { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Locator" /> class.
        /// </summary>
        public Locator()
        {
            this.ToolDirectory = ".";
        }

        /// <summary>
        /// Path to ffmpeg (on Windows).
        /// </summary>
        public const string FFmpegWindowsPath = "ffmpeg.exe";

        /// <summary>
        /// Path to ffmpeg (on anything not Windows).
        /// </summary>
        public const string FFmpegNonWindowsPath = "ffmpeg";

        /// <summary>
        /// Path to ffprobe (on Windows).
        /// </summary>
        public const string FFprobeWindowsPath = "ffprobe.exe";

        /// <summary>
        /// Path to ffprobe (on anything not Windows).
        /// </summary>
        public const string FFprobeNonWindowsPath = "ffprobe";

        /// <summary>
        /// Gets FFmpeg path (on Windows).
        /// </summary>
        /// <returns>Path to FFmpeg on Windows.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown when the operating system is not Windows.</exception>
        public FFmpegPath GetWindowsFFmpegPath()
            => OperatingSystem.IsWindows() ? new FFmpegPath()
            {
                Path = Path.Combine(this.ToolDirectory, FFmpegWindowsPath),
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), this.ToolDirectory, FFmpegWindowsPath)
            } : throw new PlatformNotSupportedException("Attempting to get FFmpeg path from a non-Windows operating system");

        /// <summary>
        /// Gets FFprobe path (on Windows).
        /// </summary>
        /// <returns>Path to FFprobe on Windows.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown when the operating system is not Windows.</exception>
        public FFmpegPath GetWindowsFFprobePath()
            => OperatingSystem.IsWindows() ? new FFmpegPath()
            {
                Path = Path.Combine(this.ToolDirectory, FFprobeWindowsPath),
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), this.ToolDirectory, FFprobeWindowsPath)
            } : throw new PlatformNotSupportedException("Attempting to get FFprobe path from a non-Windows operating system");

        /// <summary>
        /// Gets FFmpeg path (on anything not Windows).
        /// </summary>
        /// <returns>Path to FFmpeg on anything not Windows.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown when the operating system is Windows.</exception>
        public FFmpegPath GetNonWindowsFFmpegPath()
            => !OperatingSystem.IsWindows() ? new FFmpegPath()
            {
                Path = Path.Combine(this.ToolDirectory, FFmpegNonWindowsPath),
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), this.ToolDirectory, FFmpegNonWindowsPath)
            } : throw new PlatformNotSupportedException("Attempting to get FFmpeg path from a Windows operating system");

        /// <summary>
        /// Gets FFprobe path (on anything not Windows).
        /// </summary>
        /// <returns>Path to FFprobe on anything not Windows.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown when the operating system is Windows.</exception>
        public FFmpegPath GetNonWindowsFFprobePath()
            => !OperatingSystem.IsWindows() ? new FFmpegPath()
            {
                Path = Path.Combine(this.ToolDirectory, FFprobeNonWindowsPath),
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), this.ToolDirectory, FFprobeNonWindowsPath)
            } : throw new PlatformNotSupportedException("Attempting to get FFprobe path from a Windows operating system");

        /// <summary>
        /// Gets FFmpeg path based on the operating system.
        /// </summary>
        /// <returns>Path to ffmpeg.exe if being run on Windows; path to ffmpeg (without .exe) otherwise.</returns>
        public FFmpegPath GetFFmpegPath()
            => OperatingSystem.IsWindows() ? GetWindowsFFmpegPath() : GetNonWindowsFFmpegPath();

        /// <summary>
        /// Gets FFprobe path based on the operating system.
        /// </summary>
        /// <returns>Path to ffprobe.exe if being run on Windows; path to ffprobe (without .exe) otherwise.</returns>
        public FFmpegPath GetFFprobePath()
            => OperatingSystem.IsWindows() ? GetWindowsFFprobePath() : GetNonWindowsFFprobePath();
    }
}
