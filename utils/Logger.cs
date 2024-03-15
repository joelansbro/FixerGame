namespace FixerGame
{
    public class Logger
    {
        private static bool debugMode = false;

        public static void SetDebugMode(bool mode)
        {
            debugMode = mode;
        }

        public static void Log(string message)
        {
            if (debugMode)
            {
                string logEntry = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}";
                Console.WriteLine(message);
            }
        }
    }
}