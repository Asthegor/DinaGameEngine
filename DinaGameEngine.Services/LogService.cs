using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public class LogService (IFileService fileService) : ILogService
    {
        private readonly IFileService _fileService = fileService;

        public void Error(string message, int level = 0)
        {
            WriteLog("ERROR", message);
        }

        public void Info(string message, int level = 0)
        {

            WriteLog("INFO", $"{ new string(' ', 4 * level)}{message}");
        }

        public void Warning(string message, int level = 0)
        {
            WriteLog("WARNING", message);
        }

        private void WriteLog(string messagetype, string message)
        {
            var appDataFolder = _fileService.GetAppDataDirectory();
            if (!_fileService.DirectoryExists(appDataFolder))
                _fileService.CreateDirectory(appDataFolder);

            var logMessage = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] [{messagetype.ToUpper()}] {message}{Environment.NewLine}";
            var logFileName = _fileService.Combine(appDataFolder, ProjectStructure.LogFileName);
            try
            {
                _fileService.AppendAllText(logFileName, logMessage);
            }
            catch (Exception)
            {
            }
        }
    }
}
