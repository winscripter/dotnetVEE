using dotnetVEE.Abstractions.Serialization;

namespace dotnetVEE.Wrapper
{
    /// <summary>
    /// Represents a path to FFmpeg.
    /// </summary>
    public struct FFmpegPath : IBinarySerializable
    {
        /// <summary>
        /// Relative path to FFmpeg.
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// Absolute path to FFmpeg.
        /// </summary>
        public string FullPath { get; init; }
        
        /// <inheritdoc />
        public void BinarySerialize(BinaryWriter writer)
        {
            writer.Write(this.Path);
            writer.Write(this.FullPath);
        }
    }
}
