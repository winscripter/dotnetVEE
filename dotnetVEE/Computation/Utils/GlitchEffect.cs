using dotnetVEE.Abstractions;
using dotnetVEE.Abstractions.FileGeneration;
using dotnetVEE.Computation.ImageUtils;
using dotnetVEE.Private.Extensions;
using dotnetVEE.Private.Utils;
using dotnetVEE.Wrapper;
using SixLabors.ImageSharp;
using System.Collections.ObjectModel;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace dotnetVEE.Computation.Utils
{
    /// <summary>
    /// Represents configuration for the glitch effect.
    /// </summary>
    /// <param name="Timestamps">Start &amp; End Timestamps for the effect to apply.</param>
    /// <param name="Probability">Probability of each row to have glitch effect applied. This value must range between 0 and 1.</param>
    /// <param name="MinShift">Minimum value for shifting each row.</param>
    /// <param name="MaxShift">Maximum value for shifting each row.</param>
    public record GlitchConfiguration(
        StartEndTimestamp Timestamps,
        float Probability = 0.1F,
        int MinShift = -30,
        int MaxShift = 30);

    /// <summary>
    /// Glitch effect for a video.
    /// </summary>
    public class GlitchEffect
    {
        private readonly GlitchConfiguration _configuration;
        private readonly DeleteGeneratedFiles _cleanupMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlitchEffect" /> class.
        /// </summary>
        /// <param name="configuration">Configuration for the glitch effect.</param>
        /// <param name="cleanupMode">Settings for the cleanup task.</param>
        public GlitchEffect(GlitchConfiguration configuration, DeleteGeneratedFiles cleanupMode)
        {
            _configuration = configuration;
            _cleanupMode = cleanupMode;
        }

        /// <summary>
        /// Gets file names for generated files.
        /// </summary>
        public GeneratedFileNames? FileNames { get; private set; } = null;

        /// <inheritdoc />
        public void Run(Video vid, ref ObservableCollection<float> progress)
        {
            progress.Add(0.0F);

            int startIndex = (int)(_configuration.Timestamps.Start.TotalSeconds * vid.FPS);
            int endIndex = (int)(_configuration.Timestamps.End.TotalSeconds * vid.FPS);

            string dirName = RandomPathGenerator.GenerateDirectoryNameV1();
            Directory.CreateDirectory(dirName);

            new ExtractFramesWithIndex(
                startIndex,
                endIndex,
                dirName).Run(vid, ref progress);

            string tempFrameDirFormat = $"{dirName}/{Constants.PreferredIndexingName}";

            float addBy = 100F / Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)).Length;

            float currentProgress = 0.0F;

            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), dirName)))
            {
                Image image = Image.Load(file);
                
                Glitch.Apply(file, file, _configuration.Probability, _configuration.MinShift, _configuration.MaxShift);
                progress.Add(currentProgress);
                currentProgress += addBy;
            }

            string outputFileName = RandomPathGenerator.GenerateRandomFileWithExtensionV1(
                vid.Path.Contains('.') ? vid.Path.Split('.').Last() : "mp4" // falls back to mp4
            );

            string o = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{vid.Path}\" -itsoffset {_configuration.Timestamps.Start.TotalSeconds.SafeStringify()} -i \"{tempFrameDirFormat}\" -lavfi \"overlay=eof_action=pass\" \"{outputFileName}\"");

            var gfn = new GeneratedFileNames(dirName, Path.GetFullPath(dirName), outputFileName, Path.GetFullPath(outputFileName));
            Cleaner.CleanAndRestore(gfn, _cleanupMode, vid.Path);

            FileNames = gfn;

            progress.Add(100.0F);
        }
    }
}
