using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MenuTitleViewModel(ComponentModel model) : ItemViewModel(model)
    {
        public override string Icon => string.Empty;
        public override string Key => ((ComponentModel)Model).Key;
        public override string Name => ((ComponentModel)Model).Key;

        public string Font
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("Font", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
        }
        public string Content
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("Content", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
        }
        public string Color
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("Color", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
        }

        public void NotifyChanged()
        {
            OnPropertyChanged(nameof(Key));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Content));
        }
    }
}