using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;
using DinaGameEngine.Utils;

using System.Drawing;
using System.Text.Json;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuManagerComponentPropertiesViewModel : ComponentPropertiesViewModel
    {
        private int? _itemSpacingX;
        private int? _itemSpacingY;
        private int _currentItemindex;
        private DinaDirection _direction;
        public MenuManagerComponentPropertiesViewModel(ComponentModel existingComponent)
            : base(existingComponent)
        {
            ResetItemSpacingCommand = new RelayCommand(ResetItemSpacing);
            ResetCancellationCommand = new RelayCommand(ResetCancellation);

            LoadFrom(existingComponent);
            NotifyChange(false);
        }
        public static string DeleteIcon => DinaIcon.Delete.ToGlyph();
        public override bool IsValid => true;
        protected override void LoadFrom(ComponentModel source)
        {
            if (source.Properties.TryGetValue("ItemSpacing", out var itemSpacing))
            {
                if (itemSpacing is JsonElement pe)
                {
                    ItemSpacingX = pe.GetProperty("X").GetInt32();
                    ItemSpacingY = pe.GetProperty("Y").GetInt32();
                }
                else if (itemSpacing is Point pt)
                {
                    ItemSpacingX = pt.X;
                    ItemSpacingY = pt.Y;
                }
            }
            else
            {
                ItemSpacingX = null;
                ItemSpacingY = null;
            }
            CurrentItemIndex = ComponentPropertyConverter.GetIntProperty(source, "CurrentItemIndex", -1);
            Direction = ComponentPropertyConverter.GetEnumProperty(source, "Direction", DinaDirection.Vertical);
        }
        public override void ApplyToModel()
        {
            _component.Key = Key;

            if (ItemSpacingX.HasValue || ItemSpacingY.HasValue)
                _component.Properties["ItemSpacing"] = new Point(ItemSpacingX ?? 0, ItemSpacingY ?? 0);
            else
                _component.Properties.Remove("ItemSpacing");

            if (CurrentItemIndex != -1)
                _component.Properties["CurrentItemIndex"] = ComponentPropertyConverter.GetReturnValueFrom(CurrentItemIndex);
            else
                _component.Properties.Remove("CurrentItemIndex");

            if (Direction != DinaDirection.Vertical)
                _component.Properties["Direction"] = ComponentPropertyConverter.GetReturnValueFrom(Direction);
            else
                _component.Properties.Remove("Direction");
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
        #endregion
    }
}
