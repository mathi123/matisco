using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Matisco.Wpf.Controls.Buttons;

namespace Matisco.Wpf.Controls.Coverters
{
    public class DisplayMemberPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, null))
                return null;

            var path = parameter as string;

            if (string.IsNullOrEmpty(path))
            {
                return value.ToString();
            }

            var property = value.GetType().GetProperty(path);

            return property.GetValue(value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
