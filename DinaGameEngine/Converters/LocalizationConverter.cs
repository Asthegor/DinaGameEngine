using DinaGameEngine.Common;

using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    internal class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LocalizationManager.GetTranslation(value.ToString()!);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
