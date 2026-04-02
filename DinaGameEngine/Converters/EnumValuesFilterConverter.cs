using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class EnumValuesFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((IEnumerable)value)
                .Cast<object>()
                .Where(v => v.ToString() != "None" && v.ToString() != "Max")
                .ToList();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
