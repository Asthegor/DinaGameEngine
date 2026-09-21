using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

using System.Globalization;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    public class MenuActionCategoryToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is MenuActionCategory category
                ? LocalizationManager.GetTranslation($"MenuAction_{category}_Label")
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}