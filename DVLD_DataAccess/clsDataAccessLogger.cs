using System;
using System.Data.SqlClient;
using System.IO;

namespace DVLD_DataAccess
{
    public static class clsDataAccessLogger
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

            return Path.Combine(logsFolder, "Database_ErrorLog.txt");
        }

        public static void LogException(SqlException sqlEx)
        {
            string logMessage = $"[{DateTime.Now}] SQL ERROR:" +
                               $"{Environment.NewLine}Error Number: {sqlEx.Number}" +
                               $"{Environment.NewLine}Message: {sqlEx.Message}" +
                               $"{Environment.NewLine}Procedure/Source: {sqlEx.Procedure}" +
                               $"{Environment.NewLine}----------------------------------------" +
                               $"{Environment.NewLine}";

            _WriteLog(logMessage);
        }

        public static void LogException(Exception ex)
        {
            string logMessage = $"[{DateTime.Now}] GENERAL DB ERROR:" +
                               $"{Environment.NewLine}Message: {ex.Message}" +
                               $"{Environment.NewLine}StackTrace: {ex.StackTrace}" +
                               $"{Environment.NewLine}----------------------------------------" +
                               $"{Environment.NewLine}";

            _WriteLog(logMessage);
        }

        private static void _WriteLog(string logMessage)
        {
            try
            {
                string filePath = _GetLogFilePath();
                File.AppendAllText(filePath, logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logger Failed] Could not write log: {ex.Message}");
            }
        }
    }
}