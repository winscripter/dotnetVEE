namespace dotnetVEE.Private.Extensions
{
    internal static class DoubleExtensions
    {
        public static string SafeStringify(this double value) => value.ToString().Replace(",", ".");
    }
}
