using DinaGameEngine.Common.Enums;

using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class ControlVerticalAlignmentToVerticalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ControlVerticalAlignment)value switch
            {
                ControlVerticalAlignment.Top => VerticalAlignment.Top,
                ControlVerticalAlignment.Center => VerticalAlignment.Center,
                ControlVerticalAlignment.Bottom => VerticalAlignment.Bottom,
                ControlVerticalAlignment.Stretch => VerticalAlignment.Stretch,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
