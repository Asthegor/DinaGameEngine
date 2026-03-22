namespace DinaGameEngine.Abstractions
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
        string GetAppDataDirectory(); // Retourne le dossier AppData\DinaGameEngine
        string Combine(params string[] paths);
        void AppendAllText(string path, string content);
        void CreateResxFile(string path, string namespaceName, string className);
        void CreateResxDesignerFile(string path, string namespaceName, string className);
    }
}