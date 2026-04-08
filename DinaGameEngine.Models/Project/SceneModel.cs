namespace DinaGameEngine.Models.Project
{
    public class SceneModel : ItemModel
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public bool IsStartup { get; set; }
        public List<ComponentModel> Components { get; set; } = [];
#if DEBUG // TODO: à retirer une fois avoir terminé les contrôles Slider, CheckBox, ListBox, Button, Group, ShadowText
        public bool ToBeIncluded { get; set; } = true;
#endif

    }
}
