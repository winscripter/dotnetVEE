namespace dotnetVEE
{
    internal static class DebuggingHelper
    {
        private const bool allow_debugging = true;

        public static void Notice(string message)
        {
            if (allow_debugging)
            {
                File.AppendAllText("Debug.log", message + Environment.NewLine);
            }
        }
    }
}
