using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class MenuActionTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is MenuActionType type
                ? type switch
                {
                    MenuActionType.ChangeColor => DinaIcon.Color.ToGlyph(),
                    MenuActionType.ChangeScene => DinaIcon.Photo.ToGlyph(),
                    _ => string.Empty
                }
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
