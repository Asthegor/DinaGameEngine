using System.Windows.Media;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class NamedColorItem
    {
        public string Name { get; set; } = string.Empty;
        public Color Color { get; set; }

        public override string ToString() => Name;
    }
}
