using DinaGameEngine.Models.Project;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DinaGameEngine.Converters
{
    public class ColorModelToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorModel color)
                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}