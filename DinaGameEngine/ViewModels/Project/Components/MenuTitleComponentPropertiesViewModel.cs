using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuTitleComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private string _font = string.Empty;
        private string _content = string.Empty;
        private string _color = string.Empty;
        private int? _positionX;
        private int? _positionY;
        private int? _dimensionsX;
        private int? _dimensionsY;
        private int _zOrder = 0;
        private bool _visible = true;
        private DinaHorizontalAlignment _hAlign = DinaHorizontalAlignment.Left;
        private DinaVerticalAlignment _vAlign = DinaVerticalAlignment.Top;
        private bool _withShadow = false;
        private string _shadowColor = string.Empty;
        private int? _shadowOffsetX;
        private int? _shadowOffsetY;

        public MenuTitleComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts,
                                                    IEnumerable<ColorModel> availableColors,
                                                    ComponentModel component) : base(component)
        {
            AvailableFonts = availableFonts;
            AvailableColors = availableColors;

            ResetPositionCommand = new RelayCommand(ResetPosition);
            ResetDimensionsCommand = new RelayCommand(ResetDimensions);
            ResetShadowOffsetCommand = new RelayCommand(ResetShadowOffset);

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
        public int? PositionX
        {
            get => _positionX;
            set
            {
                if (_positionX == null && value != null && PositionY == null)
                    _positionY = 0;
                SetProperty(ref _positionX, value);
                NotifyChange();
            }
        }
        public int? PositionY
        {
            get => _positionY;
            set
            {
                if (_positionY == null && value != null && PositionX == null)
                    _positionX = 0;
                SetProperty(ref _positionY, value);
                NotifyChange();
            }
        }
        public int? DimensionsX
        {
            get => _dimensionsX;
            set
            {
                if (_dimensionsX == null && value != null && DimensionsY == null)
                    _dimensionsY = 0;
                SetProperty(ref _dimensionsX, value);
                NotifyChange();
            }
        }
        public int? DimensionsY
        {
            get => _dimensionsY;
            set
            {
                if (_dimensionsY == null && value != null && DimensionsX == null)
                    _dimensionsX = 0;
                SetProperty(ref _dimensionsY, value);
                NotifyChange();
            }
        }
        public int ZOrder
        {
            get => _zOrder;
            set
            {
                SetProperty(ref _zOrder, value);
                NotifyChange();
            }
        }
        public bool Visible
        {
            get => _visible;
            set
            {
                SetProperty(ref _visible, value);
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
        public bool WithShadow
        {
            get => _withShadow;
            set
            {
                SetProperty(ref _withShadow, value);
                NotifyChange();
            }
        }
        public ColorModel? SelectedShadowColor
        {
            get => AvailableColors.FirstOrDefault(c => c.Key == _shadowColor);
            set
            {
                _shadowColor = value?.Key ?? string.Empty;
                OnPropertyChanged();
                NotifyChange();
            }
        }
        public int? ShadowOffsetX
        {
            get => _shadowOffsetX;
            set
            {
                if (_shadowOffsetX == null && value != null && ShadowOffsetY == null)
                    _shadowOffsetY = 0;
                SetProperty(ref _shadowOffsetX, value);
                NotifyChange();
            }
        }
        public int? ShadowOffsetY
        {
            get => _shadowOffsetY;
            set
            {
                if (_shadowOffsetY == null && value != null && ShadowOffsetX == null)
                    _shadowOffsetX = 0;
                SetProperty(ref _shadowOffsetY, value);
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


            (PositionX, PositionY) = ComponentPropertyHelper.GetPointProperty(source, "Position");
            (DimensionsX, DimensionsY) = ComponentPropertyHelper.GetPointProperty(source, "Dimensions");

            ZOrder = ComponentPropertyHelper.GetIntProperty(source, "ZOrder", 0);
            Visible = ComponentPropertyHelper.GetBoolProperty(source, "Visible", true);

            WithShadow = ComponentPropertyHelper.GetBoolProperty(source, "WithShadow", false);
            _shadowColor = source.Properties.TryGetValue("ShadowColor", out var shadowColor) ? shadowColor?.ToString() ?? string.Empty : string.Empty;
            (ShadowOffsetX, ShadowOffsetY) = ComponentPropertyHelper.GetPointProperty(source, "ShadowOffset");
        }

        public override void ApplyToModel()
        {
            _component.Key = Key;
            _component.Properties["Font"] = _font;
            _component.Properties["Content"] = _content;
            _component.Properties["Color"] = _color;

            if (PositionX.HasValue || PositionY.HasValue)
                _component.Properties["Position"] = new Point(PositionX ?? 0, PositionY ?? 0);
            else
                _component.Properties.Remove("Position");

            if (DimensionsX.HasValue || DimensionsY.HasValue)
                _component.Properties["Dimensions"] = new Point(DimensionsX ?? 0, DimensionsY ?? 0);
            else
                _component.Properties.Remove("Dimensions");

            if (ZOrder != 0)
                _component.Properties["ZOrder"] = ComponentPropertyHelper.GetReturnValueFrom(ZOrder);
            else
                _component.Properties.Remove("ZOrder");

            if (!Visible)
                _component.Properties["Visible"] = ComponentPropertyHelper.GetReturnValueFrom(Visible);
            else
                _component.Properties.Remove("Visible");

            if (HAlign != DinaHorizontalAlignment.Left)
                _component.Properties["HAlign"] = HAlign.ToString();
            else
                _component.Properties.Remove("HAlign");

            if (VAlign != DinaVerticalAlignment.Top)
                _component.Properties["VAlign"] = VAlign.ToString();
            else
                _component.Properties.Remove("VAlign");


            if (WithShadow)
            {
                _component.Properties["WithShadow"] = ComponentPropertyHelper.GetReturnValueFrom(WithShadow);
                _component.Properties["ShadowColor"] = _shadowColor;
                if (ShadowOffsetX.HasValue || ShadowOffsetY.HasValue)
                    _component.Properties["ShadowOffset"] = new Point(ShadowOffsetX ?? 0, ShadowOffsetY ?? 0);
            }
            else
            {
                _component.Properties.Remove("WithShadow");
                _component.Properties.Remove("ShadowColor");
                _component.Properties.Remove("ShadowOffset");
            }
        }

        public RelayCommand ResetPositionCommand { get; }
        private void ResetPosition()
        {
            PositionX = null;
            PositionY = null;
        }
        public RelayCommand ResetDimensionsCommand { get; }
        private void ResetDimensions()
        {
            DimensionsX = null;
            DimensionsY = null;
        }

        public RelayCommand ResetShadowOffsetCommand { get; }
        private void ResetShadowOffset()
        {
            ShadowOffsetX = null;
            ShadowOffsetY = null;
        }
    }
}