namespace DinaGameEngine.Services
{
    public interface ILogService
    {
        public void Info(string message);
        public void Warning(string message);
        public void Error(string message);
    }
}
