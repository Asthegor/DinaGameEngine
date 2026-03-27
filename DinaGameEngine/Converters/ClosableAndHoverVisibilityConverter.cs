using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    internal class ClosableAndHoverVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isClosable = values[0] is bool b && b;
            var isMouseOver = values[1] is bool a && a;
            if (!isClosable)
                return Visibility.Collapsed;
            return isClosable && isMouseOver ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

