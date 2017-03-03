using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Matisco.Wpf.Controls.Buttons;

namespace Matisco.Wpf.Controls.Coverters
{
    public class ButtonImageEnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = (ButtonImageEnum) value;

            if (image == ButtonImageEnum.None)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
