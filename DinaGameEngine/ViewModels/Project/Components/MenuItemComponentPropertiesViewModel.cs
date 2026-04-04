using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuItemComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private string _font = string.Empty;
        private string _content = string.Empty;
        private string _color = string.Empty;
        private DinaHorizontalAlignment _hAlign = DinaHorizontalAlignment.Left;
        private DinaVerticalAlignment _vAlign = DinaVerticalAlignment.Top;
        private string _state = "Enable";

        public MenuItemComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts,
                                                    IEnumerable<ColorModel> availableColors,
                                                    ComponentModel component) : base(component)
        {
            AvailableFonts = availableFonts;
            AvailableColors = availableColors;

            LoadFrom(component);
            NotifyChange(false);
        }

        public IEnumerable<FontModel> AvailableFonts { get; }
        public IEnumerable<ColorModel> AvailableColors { get; }
        public IEnumerable<string> AvailableStates { get; } = ["Enable", "Disable"];
        public IEnumerable<DinaHorizontalAlignment> AvailableHAlignments { get; } = Enum.GetValues<DinaHorizontalAlignment>();
        public IEnumerable<DinaVerticalAlignment> AvailableVAlignments { get; } = Enum.GetValues<DinaVerticalAlignment>();

        public override bool IsValid => SelectedFont != null && SelectedColor != null;

        public FontModel? SelectedFont
        {
            get => AvailableFonts.FirstOrDefault(f => f.Key == _font);
            set
            {
                _font = value?.Key ?? string.Empty;
                OnPropertyChanged();
                NotifyChange();
            }
        }
        public ColorModel? SelectedColor
        {
            get => AvailableColors.FirstOrDefault(c => c.Key == _color);
            set
            {
                _color = value?.Key ?? string.Empty;
                OnPropertyChanged();
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
        public DinaHorizontalAlignment HAlign
        {
            get => _hAlign;
            set
            {
                SetProperty(ref _hAlign, value);
                NotifyChange();
            }
        }
        public DinaVerticalAlignment VAlign
        {
            get => _vAlign;
            set
            {
                SetProperty(ref _vAlign, value);
                NotifyChange();
            }
        }
        public string State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                NotifyChange();
            }
        }

        protected override void LoadFrom(ComponentModel source)
        {
            _font = source.Properties.TryGetValue("Font", out var font) ? font?.ToString() ?? string.Empty : string.Empty;
            _content = source.Properties.TryGetValue("Content", out var content) ? content?.ToString() ?? string.Empty : string.Empty;
            _color = source.Properties.TryGetValue("Color", out var color) ? color?.ToString() ?? string.Empty : string.Empty;
            HAlign = source.Properties.TryGetValue("HAlign", out var hAlign)
                   ? Enum.TryParse<DinaHorizontalAlignment>(hAlign?.ToString(), out var ha) ? ha : DinaHorizontalAlignment.Left
                   : DinaHorizontalAlignment.Left;
            VAlign = source.Properties.TryGetValue("VAlign", out var vAlign)
                   ? Enum.TryParse<DinaVerticalAlignment>(vAlign?.ToString(), out var va) ? va : DinaVerticalAlignment.Top
                   : DinaVerticalAlignment.Top;
            _state = source.Properties.TryGetValue("State", out var state) ? state?.ToString() ?? "Enable" : "Enable";
            OnPropertyChanged(nameof(SelectedFont));
            OnPropertyChanged(nameof(SelectedColor));
            OnPropertyChanged(nameof(State));
        }

        public override void ApplyToModel()
        {
            _component.Key = Key;
            _component.Properties["Font"] = _font;
            _component.Properties["Content"] = _content;
            _component.Properties["Color"] = _color;

            if (HAlign != DinaHorizontalAlignment.Left)
                _component.Properties["HAlign"] = HAlign.ToString();
            else
                _component.Properties.Remove("HAlign");

            if (VAlign != DinaVerticalAlignment.Top)
                _component.Properties["VAlign"] = VAlign.ToString();
            else
                _component.Properties.Remove("VAlign");

            if (State != "Enable")
                _component.Properties["State"] = State;
            else
                _component.Properties.Remove("State");
        }
    }
}