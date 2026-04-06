using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;
using System.Text.Json;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class TextComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private FontModel? _selectedFont;
        private string _content = string.Empty;
        private ColorModel? _selectedColor;
        private int? _positionX;
        private int? _positionY;
        private int? _dimensionsX;
        private int? _dimensionsY;
        private int _zOrder = 0;
        private bool _visible = true;
        private float _rotation = 0f;
        private DinaHorizontalAlignment _horizontalAlignment = DinaHorizontalAlignment.Left;
        private DinaVerticalAlignment _verticalAlignment = DinaVerticalAlignment.Top;

        public TextComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts,
                                                IEnumerable<ColorModel> availableColors,
                                                ComponentModel existingComponent)
            : base(existingComponent)
        {
            AvailableColors = availableColors;
            AvailableFonts = availableFonts;

            ResetPositionCommand = new RelayCommand(ResetPosition);
            ResetDimensionsCommand = new RelayCommand(ResetDimensions);

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

            if (source.Properties.TryGetValue("Position", out var position))
            {
                if (position is JsonElement pe)
                {
                    PositionX = pe.GetProperty("X").GetInt32();
                    PositionY = pe.GetProperty("Y").GetInt32();
                }
                else if (position is Point pt)
                {
                    PositionX = pt.X;
                    PositionY = pt.Y;
                }
            }
            else
            {
                PositionX = null;
                PositionY = null;
            }

            if (source.Properties.TryGetValue("Dimensions", out var dimensions))
            {
                if (dimensions is JsonElement de)
                {
                    DimensionsX = de.GetProperty("X").GetInt32();
                    DimensionsY = de.GetProperty("Y").GetInt32();
                }
                else if (dimensions is Point pt)
                {
                    DimensionsX = pt.X;
                    DimensionsY = pt.Y;
                }
            }
            else
            {
                DimensionsX = null;
                DimensionsY = null;
            }

            ZOrder = ComponentPropertyHelper.GetIntProperty(source, "ZOrder", 0);
            Visible = ComponentPropertyHelper.GetBoolProperty(source, "Visible", true);
            Rotation = ComponentPropertyHelper.GetFloatProperty(source, "Rotation", 0f);
            HorizontalAlignment = ComponentPropertyHelper.GetEnumProperty(source, "HorizontalAlignment", DinaHorizontalAlignment.Left);
            VerticalAlignment = ComponentPropertyHelper.GetEnumProperty(source, "VerticalAlignment", DinaVerticalAlignment.Top);
        }
        public override void ApplyToModel()
        {
            _component.Key = Key;
            _component.Properties["Font"] = SelectedFont?.Key ?? string.Empty;
            _component.Properties["Content"] = Content;
            _component.Properties["Color"] = SelectedColor?.Key ?? string.Empty;
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

            if (Rotation != 0f)
                _component.Properties["Rotation"] = ComponentPropertyHelper.GetReturnValueFrom(Rotation);
            else
                _component.Properties.Remove("Rotation");

            if (HorizontalAlignment != DinaHorizontalAlignment.Left)
                _component.Properties["HorizontalAlignment"] = ComponentPropertyHelper.GetReturnValueFrom(HorizontalAlignment);
            else
                _component.Properties.Remove("HorizontalAlignment");

            if (VerticalAlignment != DinaVerticalAlignment.Top)
                _component.Properties["VerticalAlignment"] = ComponentPropertyHelper.GetReturnValueFrom(VerticalAlignment);
            else
                _component.Properties.Remove("VerticalAlignment");
        }


        #region Propriétés
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
        public float Rotation
        {
            get => _rotation;
            set
            {
                SetProperty(ref _rotation, value);
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
        #endregion

        #region Commandes
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
        #endregion
    }
}