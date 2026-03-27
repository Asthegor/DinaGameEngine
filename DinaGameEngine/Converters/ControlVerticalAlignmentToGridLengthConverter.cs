using DinaGameEngine.Common.Enums;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class ControlVerticalAlignmentToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ControlVerticalAlignment)value == ControlVerticalAlignment.Stretch
                ? new GridLength(1, GridUnitType.Star)
                : GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
