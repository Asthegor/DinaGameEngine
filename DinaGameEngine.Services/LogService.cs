using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public class LogService (IFileService fileService) : ILogService
    {
        private readonly IFileService _fileService = fileService;

        public void Error(string message)
        {
            WriteLog("ERROR", message);
        }

        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        public void Warning(string message)
        {
            WriteLog("WARNING", message);
        }

        private void WriteLog(string level, string message)
        {
            var appDataFolder = _fileService.GetAppDataDirectory();
            if (!_fileService.DirectoryExists(appDataFolder))
                _fileService.CreateDirectory(appDataFolder);

            var logMessage = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] [{level.ToUpper()}] {message}{Environment.NewLine}";
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
