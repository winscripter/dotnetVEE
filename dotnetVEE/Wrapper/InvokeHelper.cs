using System.Diagnostics;

namespace dotnetVEE.Wrapper
{
    /// <summary>
    /// Helper to invoke FFmpeg and FFprobe.
    /// </summary>
    public static class InvokeHelper
    {
        /// <summary>
        /// Returns a starting-point instance of <see cref="Process" />, ready to use for launching FFmpeg.
        /// </summary>
        /// <returns>A new instance of <see cref="Process" /> being a template to ffmpeg.</returns>
        public static Process GetFFmpegLaunchTemplate()
        {
            Locator locator = new Locator();
            string path = locator.GetFFmpegPath().Path;

            var proc = new Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            return proc;
        }

        /// <summary>
        /// Launches FFmpeg in the background
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFmpeg
        /// </param>
        public static void LaunchFFmpeg(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFmpegPath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
        }

        /// <summary>
        /// Launches FFmpeg in the background and waits for it to close
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFmpeg
        /// </param>
        public static void LaunchAndWaitForFFmpeg(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFmpegPath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Gets the process instance template for FFprobe
        /// </summary>
        public static Process GetFFprobeLaunchTemplate()
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFprobePath().Path;
            process.StartInfo.Arguments = string.Empty;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            return process;
        }

        /// <summary>
        /// Launches FFprobe in the background
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFprobe
        /// </param>
        public static void LaunchFFprobe(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFprobePath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
        }

        /// <summary>
        /// Launches FFprobe in the background and waits for it to close
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFprobe
        /// </param>
        public static void LaunchAndWaitForFFprobe(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFprobePath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Launches FFprobe in the background with specified arguments,
        /// and once the process is done, returns its standard output.
        /// </summary>
        /// <param name="arg">Command-line arguments</param>
        /// <returns>Standard output</returns>
        public static string LaunchAndRedirectFFprobeOutput(string arg)
        {
            var template = GetFFprobeLaunchTemplate();

            template.StartInfo.Arguments = arg;
            template.StartInfo.RedirectStandardOutput = true;

            template.Start();
            string s = template.StandardOutput.ReadToEnd();
            template.WaitForExit();

            return s;
        }

        /// <summary>
        /// Launches FFmpeg in the background with specified arguments,
        /// and once the process is done, returns its standard output.
        /// </summary>
        /// <param name="arg">Command-line arguments</param>
        /// <returns>Standard output</returns>
        public static string LaunchAndRedirectFFmpegOutput(string arg)
        {
            var template = GetFFmpegLaunchTemplate();

            template.StartInfo.Arguments = arg;
            template.StartInfo.RedirectStandardOutput = true;

            template.Start();
            string s = template.StandardOutput.ReadToEnd();
            template.WaitForExit();

            return s;
        }

        /// <summary>
        /// Launches FFmpeg in the background and waits for it to close asynchronously
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFmpeg
        /// </param>
        public static async Task LaunchAndWaitForFFmpegAsync(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFmpegPath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            await process.WaitForExitAsync();
        }

        /// <summary>
        /// Launches FFprobe in the background and waits for it to close asynchronously
        /// </summary>
        /// <param name="arg">
        /// Command-line arguments to FFprobe
        /// </param>
        public static async Task LaunchAndWaitForFFprobeAsync(string arg)
        {
            var process = new Process();

            process.StartInfo.FileName = new Locator().GetFFprobePath().Path;
            process.StartInfo.Arguments = arg;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            await process.WaitForExitAsync();
        }

        /// <summary>
        /// Launches FFmpeg in the background with specified arguments,
        /// and once the process is done, returns its standard output.
        /// </summary>
        /// <param name="arg">Command-line arguments</param>
        /// <returns>Standard output</returns>
        public static async Task<string> LaunchAndRedirectFFmpegOutputAsync(string arg)
        {
            var template = GetFFmpegLaunchTemplate();

            template.StartInfo.Arguments = arg;
            template.StartInfo.RedirectStandardOutput = true;

            template.Start();
            string s = await template.StandardOutput.ReadToEndAsync();
            await template.WaitForExitAsync();

            return s;
        }

        /// <summary>
        /// Launches FFprobe in the background with specified arguments,
        /// and once the process is done, returns its standard output.
        /// </summary>
        /// <param name="arg">Command-line arguments</param>
        /// <returns>Standard output</returns>
        public static async Task<string> LaunchAndRedirectFFprobeOutputAsync(string arg)
        {
            var template = GetFFprobeLaunchTemplate();

            template.StartInfo.Arguments = arg;
            template.StartInfo.RedirectStandardOutput = true;

            template.Start();
            string s = await template.StandardOutput.ReadToEndAsync();
            await template.WaitForExitAsync();

            return s;
        }

        /// <summary>
        /// Makes sure video file exists. Throws <see cref="FileNotFoundException" />
        /// if it doesn't. In fact, this applies to any file.
        /// </summary>
        /// <param name="path">Input file (primarily video file)</param>
        /// <param name="pName">Parameter name that's passed to an exception</param>
        /// <exception cref="FileNotFoundException">Video file is not found</exception>
        public static void EnsureVideoExists(string path, string? pName = null)
        {
            pName ??= nameof(pName);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Video file cannot be found", pName);
            }
        }

        /// <summary>
        /// Throws <see cref="FileNotFoundException" /> if FFmpeg does not exist.
        /// </summary>
        public static void EnsureFFmpegExists()
        {
            if (!File.Exists(new Locator().GetFFmpegPath().FullPath))
            {
                throw new FileNotFoundException("Cannot find FFmpeg. Please make sure ffmpeg is named ffmpeg.exe if on Windows and ffmpeg if not on Windows, and it is in the same directory as the assembly.");
            }
        }

        /// <summary>
        /// Throws <see cref="FileNotFoundException" /> if FFprobe does not exist.
        /// </summary>
        public static void EnsureFFprobeExists()
        {
            if (!File.Exists(new Locator().GetFFprobePath().FullPath))
            {
                throw new FileNotFoundException("Cannot find FFprobe. Please make sure FFprobe is named ffprobe.exe if on Windows and ffprobe if not on Windows, and it is in the same directory as the assembly.");
            }
        }
    }
}
