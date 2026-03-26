using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Models.Project
{
    public class NavigationItemModel
    {
        public ProjectView View { get; set; }
        public DinaIcon Icon { get; set; }
        public string LabelKey { get; set; } = string.Empty;
    }
}
