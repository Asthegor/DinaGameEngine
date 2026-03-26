using DinaGameEngine.Common.Enums;

using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class ControlHorizontalAlignmentToHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ControlHorizontalAlignment)value switch
            {
                ControlHorizontalAlignment.Left => HorizontalAlignment.Left,
                ControlHorizontalAlignment.Center => HorizontalAlignment.Center,
                ControlHorizontalAlignment.Right => HorizontalAlignment.Right,
                ControlHorizontalAlignment.Stretch => HorizontalAlignment.Stretch,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
