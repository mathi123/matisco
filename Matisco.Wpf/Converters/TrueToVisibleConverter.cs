using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Matisco.Wpf.Converters
{
    public class TrueToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool) value ? Visibility.Visible : Visibility.Collapsed;

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility) value == Visibility.Visible;

            throw new InvalidOperationException();
        }
    }
}
