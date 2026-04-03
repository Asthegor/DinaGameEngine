using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MenuItemViewModel(MenuItemModel model) : ItemViewModel(model)
    {
        private MenuItemModel MenuItemModel => (MenuItemModel)Model;

        public override string Icon => string.Empty;
        public override string Key => MenuItemModel.Key;
        public override string Name => MenuItemModel.Key;
        public string EditableKey
        {
            get => MenuItemModel.Key;
            set
            {
                if (MenuItemModel.Key == value)
                    return;
                MenuItemModel.Key = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Key));
            }
        }
        public string Font
        {
            get => MenuItemModel.Font;
            set
            {
                if (MenuItemModel.Font == value)
                    return;
                MenuItemModel.Font = value;
                OnPropertyChanged();
            }
        }
        public string Content
        {
            get => MenuItemModel.Content;
            set
            {
                if (MenuItemModel.Content == value)
                    return;
                MenuItemModel.Content = value;
                OnPropertyChanged();
            }
        }
        public string Color
        {
            get => MenuItemModel.Color;
            set
            {
                if (MenuItemModel.Color == value)
                    return;
                MenuItemModel.Color = value;
                OnPropertyChanged();
            }
        }
        public string HAlign
        {
            get => MenuItemModel.HAlign;
            set
            {
                if (MenuItemModel.HAlign == value)
                    return;
                MenuItemModel.HAlign = value;
                OnPropertyChanged();
            }
        }
        public string VAlign
        {
            get => MenuItemModel.VAlign;
            set
            {
                if (MenuItemModel.VAlign == value)
                    return;
                MenuItemModel.VAlign = value;
                OnPropertyChanged();
            }
        }
        public string State
        {
            get => MenuItemModel.State;
            set
            {
                if (MenuItemModel.State == value)
                    return;
                MenuItemModel.State = value;
                OnPropertyChanged();
            }
        }
    }
}
