namespace DinaGameEngine.Models
{
    public class SceneModel
    {
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, ComponentModel> Components = [];
    }
}
