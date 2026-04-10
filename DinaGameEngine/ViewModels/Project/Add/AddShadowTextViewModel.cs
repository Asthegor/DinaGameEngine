using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

using System.Windows;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddShadowTextViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null)
        : ObservableObject, IAddComponentSpecificViewModel
    {
        private FontModel? _selectedFont;
        private string _content = string.Empty;
        private ColorModel? _selectedColor;
        private readonly Action? _onValidityChanged = onValidityChanged;
        private ColorModel? _selectedShadowColor;
        private int? _shadowOffsetX;
        private int? _shadowOffsetY;

        public IEnumerable<FontModel> AvailableFonts { get; } = [.. availableFonts];
        public IEnumerable<ColorModel> AvailableColors { get; } = [.. availableColors];
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
        public ColorModel? SelectedShadowColor
        {
            get => _selectedShadowColor;
            set
            {
                SetProperty(ref _selectedShadowColor, value);
                _onValidityChanged?.Invoke();
            }
        }
        public int? ShadowOffsetX
        {
            get => _shadowOffsetX;
            set
            {
                SetProperty(ref _shadowOffsetX, value);
                _onValidityChanged?.Invoke();
            }
        }
        public int? ShadowOffsetY
        {
            get => _shadowOffsetY;
            set
            {
                SetProperty(ref _shadowOffsetY, value);
                _onValidityChanged?.Invoke();
            }
        }

        public bool IsValid => SelectedFont != null && SelectedColor != null && SelectedShadowColor != null;

        public void ApplyToModel(ComponentModel component)
        {
            component.Properties["Font"] = SelectedFont?.Key ?? string.Empty;
            component.Properties["Content"] = Content;
            component.Properties["Color"] = SelectedColor?.Key ?? string.Empty;
            component.Properties["ShadowColor"] = SelectedShadowColor?.Key ?? string.Empty;
            component.Properties["ShadowOffset"] = new Point(ShadowOffsetX ?? 0, ShadowOffsetY ?? 0);
        }
    }
}
