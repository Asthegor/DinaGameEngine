using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddTextComponentViewModel : ObservableObject, IAddComponentSpecificViewModel
    {
        private FontModel? _selectedFont;
        private string _content = string.Empty;
        private ColorModel? _selectedColor;
        private readonly Action? _onValidityChanged;

        public AddTextComponentViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null)
        {
            AvailableFonts = [.. availableFonts];
            AvailableColors = [.. availableColors];
            _onValidityChanged = onValidityChanged;
        }

        public IEnumerable<FontModel> AvailableFonts { get; } = [];
        public IEnumerable<ColorModel> AvailableColors { get; } = [];
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

        public bool IsValid => SelectedFont != null && SelectedColor != null;

        public void ApplyToModel(ComponentModel component)
        {
            component.Properties["Font"] = SelectedFont?.Key ?? string.Empty;
            component.Properties["Content"] = Content;
            component.Properties["Color"] = SelectedColor?.Key ?? string.Empty;
        }
    }
}
