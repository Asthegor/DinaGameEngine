using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class NullToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string str && !string.IsNullOrEmpty(str)
                ? new Thickness(0, 0, 8, 0)  // margin quand Label présent
                : new Thickness(0);           // pas de margin quand Label absent
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
