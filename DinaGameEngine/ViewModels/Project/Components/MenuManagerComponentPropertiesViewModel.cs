using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuManagerComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private int? _itemSpacingX;
        private int? _itemSpacingY;
        private int _currentItemindex;
        private DinaDirection _direction;
        private bool _visible = true;
        private bool _iconVisible;
        private IconMenuAlignment _iconAlignment;
        private string? _iconLeftTexture;
        private string? _iconRightTexture;
        private int? _iconSpacingX;
        private bool _iconResize;
        private bool _useSharedSelectionDeselection;
        private ColorModel? _selectionColor;
        private ColorModel? _deselectionColor;

        public MenuManagerComponentPropertiesViewModel(ComponentModel existingComponent, IEnumerable<ColorModel> availableColors)
            : base(existingComponent)
        {
            ResetItemSpacingCommand = new RelayCommand(ResetItemSpacing);
            ResetCancellationCommand = new RelayCommand(ResetCancellation);

            AvailableColors = availableColors;

            LoadFrom(existingComponent);
            NotifyChange(false);
        }
        public static string DeleteIcon => DinaIcon.Delete.ToGlyph();
        public override bool IsValid => !UseSharedSelectionDeselection || (SelectionColor != null && DeselectionColor != null);
        protected override void LoadFrom(ComponentModel source)
        {
            (ItemSpacingX, ItemSpacingY) = ComponentPropertyHelper.GetPointProperty(source, "ItemSpacing");
            CurrentItemIndex = ComponentPropertyHelper.GetIntProperty(source, "CurrentItemIndex", -1);
            Direction = ComponentPropertyHelper.GetEnumProperty(source, "Direction", DinaDirection.Vertical);
            Visible = ComponentPropertyHelper.GetBoolProperty(source, "Visible", true);
            IconVisible = ComponentPropertyHelper.GetBoolProperty(source, "IconVisible", false);
            IconAlignment = ComponentPropertyHelper.GetEnumProperty(source, "IconAlignment", IconMenuAlignment.None);
            IconSpacingX = ComponentPropertyHelper.GetIntProperty(source, "IconSpacingX");
            IconLeftTexture = ComponentPropertyHelper.GetStringProperty(source, "IconLeftTexture");
            IconRightTexture = ComponentPropertyHelper.GetStringProperty(source, "IconRightTexture");
            IconResize = ComponentPropertyHelper.GetBoolProperty(source, "IconResize", false);
            UseSharedSelectionDeselection = ComponentPropertyHelper.GetBoolProperty(source, "UseSharedSelectionDeselection", false);
            SelectionColor = AvailableColors.FirstOrDefault(c => c.Key == ComponentPropertyHelper.GetStringProperty(source, "SelectionColor"));
            DeselectionColor = AvailableColors.FirstOrDefault(c => c.Key == ComponentPropertyHelper.GetStringProperty(source, "DeselectionColor"));
        }
        public override void ApplyToModel()
        {
            _component.Key = Key;

            if (ItemSpacingX.HasValue || ItemSpacingY.HasValue)
                _component.Properties["ItemSpacing"] = new Point(ItemSpacingX ?? 0, ItemSpacingY ?? 0);
            else
                _component.Properties.Remove("ItemSpacing");

            if (CurrentItemIndex != -1)
                _component.Properties["CurrentItemIndex"] = ComponentPropertyHelper.GetReturnValueFrom(CurrentItemIndex);
            else
                _component.Properties.Remove("CurrentItemIndex");

            if (Direction != DinaDirection.Vertical)
                _component.Properties["Direction"] = ComponentPropertyHelper.GetReturnValueFrom(Direction);
            else
                _component.Properties.Remove("Direction");

            if (!Visible)
                _component.Properties["Visible"] = ComponentPropertyHelper.GetReturnValueFrom(Visible);
            else
                _component.Properties.Remove("Visible");

            if (IconVisible)
                _component.Properties["IconVisible"] = ComponentPropertyHelper.GetReturnValueFrom(IconVisible);
            else
                _component.Properties.Remove("IconVisible");

            if (IconAlignment != IconMenuAlignment.None)
                _component.Properties["IconAlignment"] = ComponentPropertyHelper.GetReturnValueFrom(IconAlignment);
            else
                _component.Properties.Remove("IconAlignment");

            if (IconSpacingX.HasValue)
                _component.Properties["IconSpacingX"] = ComponentPropertyHelper.GetReturnValueFrom(IconSpacingX);
            else
                _component.Properties.Remove("IconSpacingX");

            if (!string.IsNullOrEmpty(IconLeftTexture))
                _component.Properties["IconLeftTexture"] = ComponentPropertyHelper.GetReturnValueFrom(IconLeftTexture);
            else
                _component.Properties.Remove("IconLeftTexture");

            if (!string.IsNullOrEmpty(IconRightTexture))
                _component.Properties["IconRightTexture"] = ComponentPropertyHelper.GetReturnValueFrom(IconRightTexture);
            else
                _component.Properties.Remove("IconRightTexture");

            if (IconResize)
                _component.Properties["IconResize"] = ComponentPropertyHelper.GetReturnValueFrom(IconResize);
            else
                _component.Properties.Remove("IconResize");

            _component.Properties["UseSharedSelectionDeselection"] = UseSharedSelectionDeselection;
            if (UseSharedSelectionDeselection && !string.IsNullOrEmpty(SelectionColor?.Key) && !string.IsNullOrEmpty(DeselectionColor?.Key))
            {
                _component.Properties["SelectionColor"] = SelectionColor!.Key;
                _component.Properties["DeselectionColor"] = DeselectionColor!.Key;
            }
            else
            {
                _component.Properties.Remove("SelectionColor");
                _component.Properties.Remove("DeselectionColor");
            }
        }

        #region Commandes
        public RelayCommand ResetItemSpacingCommand { get; }
        private void ResetItemSpacing()
        {
            ItemSpacingX = null;
            ItemSpacingY = null;
        }

        public RelayCommand ResetCancellationCommand { get; }
        private void ResetCancellation()
        { }
        #endregion

        #region Propriétés
        public IEnumerable<ColorModel> AvailableColors { get; }
        public int? ItemSpacingX
        {
            get => _itemSpacingX;
            set
            {
                if (_itemSpacingX == null && value != null && ItemSpacingY == null)
                    _itemSpacingY = 0;
                SetProperty(ref _itemSpacingX, value);
                NotifyChange();
            }
        }
        public int? ItemSpacingY
        {
            get => _itemSpacingY;
            set
            {
                if (_itemSpacingY == null && value != null && ItemSpacingX == null)
                    _itemSpacingX = 0;
                SetProperty(ref _itemSpacingY, value);
                NotifyChange();
            }
        }
        public int CurrentItemIndex
        {
            get => _currentItemindex;
            set
            {
                SetProperty(ref _currentItemindex, value);
                NotifyChange();
            }
        }
        public DinaDirection Direction
        {
            get => _direction;
            set
            {
                SetProperty(ref _direction, value);
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
        public IEnumerable<string> AvailableImages { get; } = [];
        public bool IconVisible
        {
            get => _iconVisible;
            set
            {
                SetProperty(ref _iconVisible, value);
                NotifyChange();
            }
        }
        public IconMenuAlignment IconAlignment
        {
            get => _iconAlignment;
            set
            {
                SetProperty(ref _iconAlignment, value);
                NotifyChange();
            }
        }
        public int? IconSpacingX
        {
            get => _iconSpacingX;
            set
            {
                SetProperty(ref _iconSpacingX, value);
                NotifyChange();
            }
        }
        public string? IconLeftTexture
        {
            get => _iconLeftTexture;
            set
            {
                SetProperty(ref _iconLeftTexture, value);
                NotifyChange();
            }
        }
        public string? IconRightTexture
        {
            get => _iconRightTexture;
            set
            {
                SetProperty(ref _iconRightTexture, value);
                NotifyChange();
            }
        }
        public bool IconResize
        {
            get => _iconResize;
            set
            {
                SetProperty(ref _iconResize, value);
                NotifyChange();
            }
        }
        public bool UseSharedSelectionDeselection
        {
            get => _useSharedSelectionDeselection;
            set
            {
                SetProperty(ref _useSharedSelectionDeselection, value);
                NotifyChange();
            }
        }
        public ColorModel? SelectionColor
        {
            get => _selectionColor;
            set
            {
                SetProperty(ref _selectionColor, value);
                NotifyChange();
            }
        }
        public ColorModel? DeselectionColor
        {
            get => _deselectionColor;
            set
            {
                SetProperty(ref _deselectionColor, value);
                NotifyChange();
            }
        }

        #endregion
    }
}
