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
            get => GetPropertyValue("Font");
        }
        public string Content
        {
            get => GetPropertyValue("Content");
        }
        public string Color
        {
            get => GetPropertyValue("Color");
        }

        public void NotifyChanged()
        {
            OnPropertyChanged(nameof(Key));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Content));
        }
    }
}