using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public class LogService : ILogService
    {
        private readonly string _logFileName;
        private readonly IFileService _fileService;

        public LogService(IFileService fileService)
        {
            _fileService = fileService;
            var logFileName = $"{ProjectStructure.LogFilePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}{ProjectStructure.LogFileExtension}";
            _logFileName = _fileService.Combine(_fileService.GetAppDataDirectory(), logFileName);
            PurgeOldLogs();
        }

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

            try
            {
                _fileService.AppendAllText(_logFileName, logMessage);
            }
            catch (Exception)
            {
            }
        }
        private void PurgeOldLogs()
        {
            var files = _fileService.GetFiles(_fileService.GetAppDataDirectory(), "log_*.txt")
                                    .OrderByDescending(f => f)
                                    .ToList();
            for (int index = ProjectStructure.LogFileMaxCount - 1; index < files.Count; index++)
                _fileService.DeleteFile(files[index]);
        }
    }
}
