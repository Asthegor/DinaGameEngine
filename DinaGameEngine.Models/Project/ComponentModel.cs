using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Models.Project
{
    public class ComponentModel : ItemModel
    {
        public string Type { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, object> Properties { get; set; } = [];
        public List<ComponentModel> SubComponents { get; set; } = [];
    }
}
