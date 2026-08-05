using System.Globalization;
using System.Text.Json;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class PointToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0.0;

            if (value is JsonElement element && element.ValueKind == JsonValueKind.Number)
                return element.GetDouble();

            if (double.TryParse(value.ToString(), out double result))
                return result;

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
