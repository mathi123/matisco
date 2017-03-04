using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    public class TrueToVisibleConverter : IValueConverter
    {
        public bool Collapse { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Visibility.Visible : Collapse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
