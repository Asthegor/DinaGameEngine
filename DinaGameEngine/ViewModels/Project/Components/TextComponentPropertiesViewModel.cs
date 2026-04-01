using DinaGameEngine.Models.Project;

using System.Windows.Input;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class TextComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private FontModel? _selectedFont;
        private string _content = string.Empty;
        private ColorModel? _selectedColor;
        public TextComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts,
                                                IEnumerable<ColorModel> availableColors,
                                                ComponentModel existingComponent)
            : base(existingComponent)
        {
            AvailableColors = availableColors;
            AvailableFonts = availableFonts;

            LoadFrom(existingComponent);
            NotifyChange(false);
        }

        public override bool IsValid => SelectedFont != null && SelectedColor != null;

        protected override void LoadFrom(ComponentModel source)
        {
            SelectedFont = source.Properties.TryGetValue("Font", out var font)
                ? AvailableFonts.FirstOrDefault(f => f.Key == font?.ToString())
                : null;
            Content = source.Properties.TryGetValue("Content", out var content)
                ? content?.ToString() ?? string.Empty
                : string.Empty;
            SelectedColor = source.Properties.TryGetValue("Color", out var color)
                ? AvailableColors.FirstOrDefault(c => c.Key == color?.ToString())
                : null;
        }
        public override void ApplyToModel()
        {
            _component.Key = Key;
            _component.Properties["Font"] = SelectedFont?.Key ?? string.Empty;
            _component.Properties["Content"] = Content;
            _component.Properties["Color"] = SelectedColor?.Key ?? string.Empty;
        }

        public IEnumerable<FontModel> AvailableFonts { get; }
        public IEnumerable<ColorModel> AvailableColors { get; }
        public FontModel? SelectedFont
        {
            get => _selectedFont;
            set
            {
                SetProperty(ref _selectedFont, value);
                NotifyChange();
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value);
                NotifyChange();
            }
        }
        public ColorModel? SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
                NotifyChange();
            }
        }
    }
}