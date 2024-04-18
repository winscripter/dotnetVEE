using dotnetVEE.Wrapper;

namespace dotnetVEE.Computation
{
    internal static class PrivateVideoExtensions
    {
        public static void ExtractFrameAt(this Video video, int frameIndex, string @as)
            => _ = InvokeHelper.LaunchAndRedirectFFmpegOutput($"-i \"{video.Path}\" -vf \"select=eq(n\\,{frameIndex})\" -vframes 1 \"{@as}\"");
    }
}
