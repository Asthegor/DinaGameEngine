using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class EqualityToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;
            // Compare l'item actuel (DataContext) avec le SelectedProject du ViewModel parent
            return values[0] == values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}