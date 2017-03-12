using System;
using System.Globalization;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    internal class DecimalToTextMultivalueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var prefix = values[0] as string;
            var suffix = values[5] as string;
            var newValue = (decimal)values[1];
            var numberGroupSeparator = values[2] as string;
            var numberDecimalSeparator = values[3] as string;
            var roundReadOnly = (int)values[4];

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = numberGroupSeparator;
            nfi.NumberDecimalSeparator = numberDecimalSeparator;

            var decimalText = Math.Round(newValue, roundReadOnly).ToString("n", nfi).TrimEnd('0');
            if (!string.IsNullOrEmpty(numberDecimalSeparator) && decimalText.EndsWith(numberDecimalSeparator))
            {
                decimalText = decimalText.Substring(0, decimalText.Length - numberDecimalSeparator.Length);
            }

            if (!string.IsNullOrEmpty(prefix))
                prefix += " ";

            if (!string.IsNullOrEmpty(suffix))
                suffix = " " + suffix;

            var text = $"{prefix}{decimalText}{suffix}";

            return text;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
