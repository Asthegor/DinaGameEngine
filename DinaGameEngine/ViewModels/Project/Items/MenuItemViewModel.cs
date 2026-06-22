using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MenuItemViewModel(ComponentModel model) : ItemViewModel(model)
    {
        public override string Icon => string.Empty;
        public override string Key => ((ComponentModel)Model).Key;
        public override string Name => ((ComponentModel)Model).Key;
        public string Font
        {
            get => GetPropertyValue("Font");
            set => SetPropertyValue("Font", value);
        }
        public string Content
        {
            get => GetPropertyValue("Content");
            set => SetPropertyValue("Content", value);
        }
        public string Color
        {
            get => GetPropertyValue("Color");
            set => SetPropertyValue("Color", value);
        }
        public string HorizontalAlignment
        {
            get => GetPropertyValue("HorizontalAlignment");
            set => SetPropertyValue("HorizontalAlignment", value);
        }
        public string VerticalAlignment
        {
            get => GetPropertyValue("VerticalAlignment");
            set => SetPropertyValue("VerticalAlignment", value);
        }
        public string State
        {
            get => GetPropertyValue("State");
            set => SetPropertyValue("State", value);
        }
        public string Action
        {
            get => GetPropertyValue("Action");
            set => SetPropertyValue("Action", value);
        }
        public void NotifyChanged()
        {
            OnPropertyChanged(nameof(Key));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Content));
        }

    }
}
