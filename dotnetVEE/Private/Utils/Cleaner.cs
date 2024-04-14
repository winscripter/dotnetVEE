using dotnetVEE.Abstractions.FileGeneration;

namespace dotnetVEE.Private.Utils
{
    /// <summary>
    /// A helper class that handles basic clean-up operations.
    /// </summary>
    internal static class Cleaner
    {
        /// <summary>
        /// Cleans up files and directories.
        /// </summary>
        /// <param name="fileNames">File and directory names</param>
        /// <param name="options">Things to clean up</param>
        public static void Clean(
            GeneratedFileNames? fileNames,
            DeleteGeneratedFiles options)
        {
            if (fileNames is null)
            {
                return;
            }

            switch (options)
            {
                case DeleteGeneratedFiles.Both:
                    CleanVideoFile();
                    CleanFramesDirectory();
                    break;
                case DeleteGeneratedFiles.FramesDirectoryOnly:
                    CleanFramesDirectory();
                    break;
                case DeleteGeneratedFiles.VideoOnly:
                    CleanVideoFile();
                    break;
                case DeleteGeneratedFiles.None:
                default:
                    break;
            }

            void CleanVideoFile()
            {
                if (fileNames.Value.VideoFileFullPath != null)
                {
                    if (File.Exists(fileNames.Value.VideoFileFullPath))
                    {
                        File.Delete(fileNames.Value.VideoFileFullPath);
                    }
                }
            }

            void CleanFramesDirectory()
            {
                if (fileNames.Value.FramesDirectoryFullPath != null)
                {
                    if (Directory.Exists(fileNames.Value.FramesDirectoryFullPath))
                    {
                        Directory.Delete(fileNames.Value.FramesDirectoryFullPath, true);
                    }
                }
            }
        }

        public static void CleanAndRestore(
            GeneratedFileNames? fileNames,
            DeleteGeneratedFiles options,
            string? videoName = null)
        {
            if (options == DeleteGeneratedFiles.None) return;

            Clean(fileNames, options == DeleteGeneratedFiles.None ? DeleteGeneratedFiles.None
                                                                  : DeleteGeneratedFiles.FramesDirectoryOnly);

            if (options == DeleteGeneratedFiles.FramesDirectoryOnly || options == DeleteGeneratedFiles.None)
            {
                return;
            }

            if (videoName != null)
            {
                if (File.Exists(videoName))
                {
                    File.Delete(videoName);
                    File.Move(fileNames?.VideoFileName ?? throw new ArgumentNullException(nameof(fileNames)), videoName);
                    File.Delete(fileNames?.VideoFileName!);
                }
            }
        }
    }
}
