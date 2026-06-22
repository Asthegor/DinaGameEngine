using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuItemComponentPropertiesViewModel : ComponentPropertiesViewModel
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
        private DinaHorizontalAlignment _horizontalAlignment = DinaHorizontalAlignment.Left;
        private DinaVerticalAlignment _verticalAlignment = DinaVerticalAlignment.Top;
        private string _state = "Enable";
        private string _action = string.Empty;

        public MenuItemComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts,
                                                    IEnumerable<ColorModel> availableColors,
                                                    ComponentModel component) : base(component)
        {
            AvailableFonts = availableFonts;
            AvailableColors = availableColors;

            ResetPositionCommand = new RelayCommand(ResetPosition);
            ResetDimensionsCommand = new RelayCommand(ResetDimensions);

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

        public DinaHorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                SetProperty(ref _horizontalAlignment, value);
                NotifyChange();
            }
        }
        public DinaVerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                SetProperty(ref _verticalAlignment, value);
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

        public string Action
        {
            get => _action;
            set
            {
                SetProperty(ref _action, value);
                NotifyChange();
            }
        }

        protected override void LoadFrom(ComponentModel source)
        {
            _font = source.Properties.TryGetValue("Font", out var font) ? font?.ToString() ?? string.Empty : string.Empty;
            _content = source.Properties.TryGetValue("Content", out var content) ? content?.ToString() ?? string.Empty : string.Empty;
            _color = source.Properties.TryGetValue("Color", out var color) ? color?.ToString() ?? string.Empty : string.Empty;
            HorizontalAlignment = ComponentPropertyHelper.GetEnumProperty(source, "HorizontalAlignment", DinaHorizontalAlignment.Left);
            VerticalAlignment = ComponentPropertyHelper.GetEnumProperty(source, "VerticalAlignment", DinaVerticalAlignment.Top);
            _state = source.Properties.TryGetValue("State", out var state) ? state?.ToString() ?? "Enable" : "Enable";
            _action = source.Properties.TryGetValue("Action", out var action) ? action?.ToString() ?? string.Empty : string.Empty;


            (PositionX, PositionY) = ComponentPropertyHelper.GetPointProperty(source, "Position");
            (DimensionsX, DimensionsY) = ComponentPropertyHelper.GetPointProperty(source, "Dimensions");

            ZOrder = ComponentPropertyHelper.GetIntProperty(source, "ZOrder", 0);
            Visible = ComponentPropertyHelper.GetBoolProperty(source, "Visible", true);
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

            if (HorizontalAlignment != DinaHorizontalAlignment.Left)
                _component.Properties["HorizontalAlignment"] = HorizontalAlignment.ToString();
            else
                _component.Properties.Remove("HorizontalAlignment");

            if (VerticalAlignment != DinaVerticalAlignment.Top)
                _component.Properties["VerticalAlignment"] = VerticalAlignment.ToString();
            else
                _component.Properties.Remove("VerticalAlignment");

            if (State != "Enable")
                _component.Properties["State"] = State;
            else
                _component.Properties.Remove("State");

            if (Action != string.Empty)
                _component.Properties["Action"] = Action;
            else
                _component.Properties.Remove("Action");
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

    }
}