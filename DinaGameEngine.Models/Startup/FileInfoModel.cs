namespace DinaGameEngine.Models.Startup
{
    public class FileInfoModel(string rootPath)
    {
        public string FileName { get; set; } = string.Empty;
        public string Directory { get; set; } = string.Empty;
        public string RootPath { get; } = rootPath;
        public string FullName => Path.Combine(RootPath, Directory, FileName);
    }
}
