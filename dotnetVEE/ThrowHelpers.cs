namespace dotnetVEE
{
    internal static class ThrowHelpers
    {
        public static void FileNotFoundException(string file)
            => _ = File.Exists(file) ? file : throw new FileNotFoundException($"Cannot find \"{file}\"");
    }
}
