using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;

using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class MenuActionTypeToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is MenuActionType type
                ? LocalizationManager.GetTranslation($"MenuAction_{type}_Label")
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
