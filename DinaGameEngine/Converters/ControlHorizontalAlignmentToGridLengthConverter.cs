using DinaGameEngine.Common.Enums;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class ControlHorizontalAlignmentToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ControlHorizontalAlignment)value == ControlHorizontalAlignment.Stretch
                ? new GridLength(1, GridUnitType.Star)
                : GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
