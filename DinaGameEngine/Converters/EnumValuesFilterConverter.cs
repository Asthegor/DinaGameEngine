using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class EnumValuesFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keepValues = parameter?.ToString()?.Split(',') ?? [];
            return ((IEnumerable)value)
                .Cast<object>()
                .Where(v => v.ToString() != "Max" &&
                            (v.ToString() != "None" || keepValues.Contains("None")))
                .ToList();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
