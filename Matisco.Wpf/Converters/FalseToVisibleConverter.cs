using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Matisco.Wpf.Converters
{
    public class FalseToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool) value ? Visibility.Collapsed : Visibility.Visible;

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility) value != Visibility.Visible;

            throw new InvalidOperationException();
        }
    }
}
