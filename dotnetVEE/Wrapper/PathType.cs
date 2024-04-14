namespace dotnetVEE.Wrapper
{
    /// <summary>
    /// Represents a type of a path.
    /// </summary>
    [Flags]
    public enum PathType : byte
    {
        /// <summary>
        /// Relative path (f.e. ./data/cats)
        /// </summary>
        Relative = 0b01000000,

        /// <summary>
        /// Absolute path (f.e. C:/Users/User/data/cats)
        /// </summary>
        Absolute = 0b10000000,
    }
}
