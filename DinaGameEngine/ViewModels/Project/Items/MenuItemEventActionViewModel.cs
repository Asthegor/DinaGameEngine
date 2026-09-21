using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MenuItemEventActionViewModel(ComponentModel model, Func<MenuActionType, IEnumerable<string>> resolveAvailableKeys) : ItemViewModel(model)
    {
        private readonly Func<MenuActionType, IEnumerable<string>> _resolveAvailableKeys = resolveAvailableKeys;

        private ComponentModel ComponentModel => (ComponentModel)Model;

        public MenuActionType ActionType => MenuItemEventHelper.GetActionType(ComponentModel);

        public override string Name => Label;
        public override string Icon => ActionType switch
        {
            MenuActionType.ChangeColor => DinaIcon.Color.ToGlyph(),
            MenuActionType.ChangeScene => DinaIcon.Photo.ToGlyph(),
            _ => string.Empty
        };
        public override string Key => SelectedKey;

        public string Label => ActionType switch
        {
            MenuActionType.ChangeColor => LocalizationManager.GetTranslation("MenuAction_ChangeColor_Label"),
            MenuActionType.ChangeScene => LocalizationManager.GetTranslation("MenuAction_ChangeScene_Label"),
            _ => ActionType.ToString()
        };

        public IEnumerable<string> AvailableKeys
            => new[] { string.Empty }.Concat(_resolveAvailableKeys(ActionType));

        public string SelectedKey
        {
            get => ComponentModel.Key;
            set
            {
                if (ComponentModel.Key == value)
                    return;
                ComponentModel.Key = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Key));
                OnPropertyChanged(nameof(HasValue));
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool HasValue => !string.IsNullOrEmpty(SelectedKey);

        public event EventHandler? Changed;
    }
}