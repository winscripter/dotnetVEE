namespace dotnetVEE.Private.Extensions
{
    internal static class TimeSpanExtensions
    {
        public static string Stringify(this TimeSpan ts)
            => $"{ts.Hours.ToString().PadLeft(2, '0')}:{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.TotalSeconds.ToString().Replace(',', '.')}";
    }
}
