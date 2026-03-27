namespace DinaGameEngine.Models.Project
{
    public class SceneModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, ComponentModel> Components = [];
    }
}
