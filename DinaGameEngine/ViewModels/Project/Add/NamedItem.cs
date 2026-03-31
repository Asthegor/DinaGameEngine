namespace DinaGameEngine.ViewModels.Project.Add
{
    public class NamedItem<T>
    {
        public string Name { get; set; } = string.Empty;
        public required T Item { get; set; }
        public override string ToString() => Name;
    }
}
