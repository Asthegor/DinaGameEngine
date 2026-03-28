namespace DinaGameEngine.Models.Project
{
    public class ComponentModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, object> Properties { get; set; } = [];
    }
}
