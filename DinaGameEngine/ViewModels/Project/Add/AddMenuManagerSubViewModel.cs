using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddMenuManagerSubViewModel : ObservableObject, IAddComponentSpecificViewModel
    {
        private FontModel? _selectedFont;
        private string _content = string.Empty;
        private ColorModel? _selectedColor;
        private readonly Action? _onValidityChanged;

        public AddMenuManagerSubViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null)
        {
            _onValidityChanged = onValidityChanged;
            AvailableFonts = [.. availableFonts];
            AvailableColors = [.. availableColors];
        }

        public IEnumerable<FontModel> AvailableFonts { get; }
        public IEnumerable<ColorModel> AvailableColors { get; }

        public FontModel? SelectedFont
        {
            get => _selectedFont;
            set
            {
                SetProperty(ref _selectedFont, value);
                _onValidityChanged?.Invoke();
            }
        }
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        public ColorModel? SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
                _onValidityChanged?.Invoke();
            }
        }

        public virtual bool IsValid => SelectedFont != null && SelectedColor != null;

        public void ApplyToModel(ComponentModel component)
        {
            component.Properties["Font"] = SelectedFont?.Key ?? string.Empty;
            component.Properties["Content"] = Content;
            component.Properties["Color"] = SelectedColor?.Key ?? string.Empty;
        }
    }
}