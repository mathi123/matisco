using System;
using System.Globalization;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    internal class TextToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal editValue;
            var parsingSuccess = decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }
    }
}
