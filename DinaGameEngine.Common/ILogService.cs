namespace DinaGameEngine.Common
{
    public interface ILogService
    {
        public void Info(string message, int level = 0);
        public void Warning(string message, int level = 0);
        public void Error(string message, int level = 0);
    }
}
