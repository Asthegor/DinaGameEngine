namespace DinaGameEngine.Models.Startup
{
    public class NewProjectModel
    {
        public string Name { get; set; } = string.Empty;
        public string ParentFolderPath { get; set; } = string.Empty;
        public string NameNoSpace => Name.Replace(" ", "");

        public string RootNamespace { get; set; } = string.Empty;
    }
}
