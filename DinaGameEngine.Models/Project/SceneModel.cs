namespace DinaGameEngine.Models.Project
{
    public class SceneModel : ItemModel
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public List<ComponentModel> Components { get; set; } = [];
    }
}
