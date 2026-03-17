namespace DinaGameEngine.Models
{
    public class NewProjectModel
    {
        public string Name { get; set; } = string.Empty;
        public string ParentFolderPath { get; set; } = string.Empty;
        public string NameNoSpace => Name.Replace(" ", "");
    }
}
