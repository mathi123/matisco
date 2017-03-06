using System;
using System.Globalization;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    public class DisplayMemberPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(values, null))
                return null;

            if (values.Length == 1)
                return values[0]?.ToString();

            var displayMemberPath = values[1].ToString();
            var value = values[0];

            if (ReferenceEquals(value, null))
                return "";

            if (string.IsNullOrEmpty(displayMemberPath))
            {
                return value.ToString();
            }

            var property = value.GetType().GetProperty(displayMemberPath);

            return property.GetValue(value).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {};
        }
    }
}
