using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class BoolToMaximizedIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == false) ? DinaIcon.Maximize.ToGlyph() : DinaIcon.Restore.ToGlyph();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
