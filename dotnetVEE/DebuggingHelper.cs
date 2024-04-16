namespace dotnetVEE
{
    internal static class DebuggingHelper
    {
        private const bool allow_debugging = false;

#pragma warning disable CS0162
        public static void Notice(string message)
        {
            if (allow_debugging)
            {
                File.AppendAllText("Debug.log", message + Environment.NewLine);
            }
        }
#pragma warning restore CS0162
    }
}
