namespace dotnetVEE.Private.Extensions
{
    internal static class FloatExtensions
    {
        public static string SafeStringify(this float value)
            => value.ToString().Replace(",", ".");
    }
}
