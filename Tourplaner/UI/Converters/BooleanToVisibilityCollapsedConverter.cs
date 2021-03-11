using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tourplaner.UI.Converters
{
    public sealed class BooleanToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool boolValue)
            {
                if (boolValue)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            throw new InvalidOperationException($"value must be {typeof(bool)}.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Visibility visibility)
            {
                if (visibility == Visibility.Visible)
                    return true;
                else
                    return false;
            }

            throw new InvalidOperationException($"value must be {typeof(Visibility)}.");
        }
    }
}
