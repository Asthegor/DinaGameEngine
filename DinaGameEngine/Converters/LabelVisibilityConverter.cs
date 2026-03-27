using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DinaGameEngine.Converters
{
    class LabelVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isLabelPresent = values[0] is string strLabel && !string.IsNullOrEmpty(strLabel);
            var isCollapsed = values[1] is bool b && b;
            return !isLabelPresent  || isCollapsed ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
