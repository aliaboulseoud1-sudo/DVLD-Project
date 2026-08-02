using System;
using System.IO;

namespace DVLD_Buisness
{
    public static class clsLogger
    {
        private static string _GetLogFilePath()
        {
            DirectoryInfo currentDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (currentDir != null && currentDir.GetFiles("*.csproj").Length == 0)
            {
                currentDir = currentDir.Parent;
            }

            string projectDirectory = currentDir?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;

            string logsFolder = Path.Combine(projectDirectory, "Logs");

            if (!Directory.Exists(logsFolder))
            {
                Directory.CreateDirectory(logsFolder);
            }

            return Path.Combine(logsFolder, "Business_ErrorLog.txt");
        }

        public static void LogToTextFile(Exception ex)
        {
            string logMessage = $"[{DateTime.Now}] BUSINESS ERROR:" +
                               $"{Environment.NewLine}Message: {ex.Message}" +
                               $"{Environment.NewLine}StackTrace: {ex.StackTrace}" +
                               $"{Environment.NewLine}----------------------------------------" +
                               $"{Environment.NewLine}";

            try
            {
                string filePath = _GetLogFilePath();
                File.AppendAllText(filePath, logMessage);
            }
            catch (Exception loggerEx)
            {
                Console.WriteLine($"[Business Logger Failed] {loggerEx.Message}");
            }
        }
    }
}