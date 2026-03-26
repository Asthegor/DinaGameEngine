namespace DinaGameEngine.Models.Project
{
    public class ComponentModel
    {
        public string Type { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, object> Properties { get; set; } = [];
    }
}
