namespace DinaGameEngine.Services
{
    public class FileService : IFileService
    {
        private const string AppFolderName = "DinaGameEngine";

        public bool FileExists(string path) => File.Exists(path);
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string content) => File.WriteAllText(path, content);

        public string GetAppDataDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, AppFolderName);
        }
        public void DeleteFile(string path) => File.Delete(path);
        public void CopyFile(string source, string destination) => File.Copy(source, destination);
        public IEnumerable<string> GetFiles(string path, string searchPattern) => Directory.GetFiles(path, searchPattern);

        //public string Combine(string path1, string path2) => Path.Combine(path1, path2);
        public string Combine(params string[] paths) => Path.Combine(paths);
        public void AppendAllText(string path, string content) => File.AppendAllText(path, content);
    }
}