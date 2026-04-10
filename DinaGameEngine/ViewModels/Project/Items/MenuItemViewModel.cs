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
            get => ((ComponentModel)Model).Properties.TryGetValue("Font", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("Font", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["Font"] = value;
                OnPropertyChanged();
            }
        }
        public string Content
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("Content", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("Content", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["Content"] = value;
                OnPropertyChanged();
            }
        }
        public string Color
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("Color", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("Color", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["Color"] = value;
                OnPropertyChanged();
            }
        }
        public string HorizontalAlignment
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("HorizontalAlignment", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("HorizontalAlignment", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["HorizontalAlignment"] = value;
                OnPropertyChanged();
            }
        }
        public string VerticalAlignment
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("VerticalAlignment", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("VerticalAlignment", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["VerticalAlignment"] = value;
                OnPropertyChanged();
            }
        }
        public string State
        {
            get => ((ComponentModel)Model).Properties.TryGetValue("State", out var val)
                   ? val?.ToString() ?? string.Empty
                   : string.Empty;
            set
            {
                if ((string)((ComponentModel)Model).Properties.GetValueOrDefault("State", string.Empty) == value)
                    return;
                ((ComponentModel)Model).Properties["State"] = value;
                OnPropertyChanged();
            }
        }
        public void NotifyChanged()
        {
            OnPropertyChanged(nameof(Key));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Content));
        }
    }
}
