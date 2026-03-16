namespace DinaGameEngine.Services
{
    public interface IFileService
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
        void DeleteFile(string path);
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        void CopyFile(string source, string destination);
        IEnumerable<string> GetFiles(string path, string searchPattern);
        // Retourne le dossier AppData\DinaGameEngine
        string GetAppDataDirectory();
        //string Combine(string path1, string path2);
        string Combine(params string[] paths);
        void AppendAllText(string path, string content);
    }
}