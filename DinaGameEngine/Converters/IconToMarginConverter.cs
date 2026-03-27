using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    internal class IconToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isIconPresent = values[0] is string strIcon && !string.IsNullOrEmpty(strIcon);
            var isIconLeft = values[1] is bool b && b;
            if (!isIconPresent)
                return new Thickness(0);
            return isIconLeft ? new Thickness(8, 0, 0, 0) : new Thickness(0, 0, 8, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
