using System;
using System.Globalization;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    internal class NumberToTextMultivalueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var prefix = values[0] as string;
            var suffix = values[5] as string;

            var newValue = values[1];
            var numberGroupSeparator = values[2] as string;
            var numberDecimalSeparator = values[3] as string;
            var roundReadOnly = (int)values[4];

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = numberGroupSeparator;
            nfi.NumberDecimalSeparator = numberDecimalSeparator;

            string text = "";
            if(newValue is decimal)
            {
                text = Math.Round((decimal) newValue, roundReadOnly).ToString("n", nfi);
            }
            else if(newValue is double)
            {
                text = Math.Round((double)newValue, roundReadOnly).ToString("n", nfi);
            }
            else if (newValue is long)
            {
                text = ((long)newValue).ToString("n", nfi);
            }
            else if (newValue is int)
            {
                text = ((int)newValue).ToString("n", nfi);
            }
            else if (newValue is short)
            {
                text =((short)newValue).ToString("n", nfi);
            }

            if (!string.IsNullOrEmpty(numberDecimalSeparator))
            {
                if (text.Contains(numberDecimalSeparator))
                {
                    text = text.TrimEnd('0');
                }

                if (text.EndsWith(numberDecimalSeparator))
                {
                    text = text.Substring(0, text.Length - numberDecimalSeparator.Length);
                }
            }

            if (!string.IsNullOrEmpty(prefix))
                prefix += " ";

            if (!string.IsNullOrEmpty(suffix))
                suffix = " " + suffix;

            var result = $"{prefix}{text}{suffix}";

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
