using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    internal class TextToNumberConverter : IMultiValueConverter
    {
        public Type TargetType { get; set; }

        public object ConvertBack(object value)
        {
            if (ReferenceEquals(TargetType, null))
                return null;

            var target = TargetType;

            if (target == typeof(decimal))
            {
                return ParseDecimal(value as string);
            }
            if (target == typeof(double))
            {
                return ParseDouble(value as string);
            }
            if (target == typeof(long))
            {
                return ParseLong(value as string);
            }
            if (target == typeof(int))
            {
                return ParseInt(value as string);
            }
            if (target == typeof(short))
            {
                return ParseShort(value as string);
            }

            throw new InvalidOperationException();
        }

        private decimal ParseDecimal(string text)
        {
            decimal editValue;
            var parsingSuccess = decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }

        private double ParseDouble(string text)
        {
            double editValue;
            var parsingSuccess = double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }

        private long ParseLong(string text)
        {
            long editValue;
            var parsingSuccess = long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }

        private int ParseInt(string text)
        {
            int editValue;
            var parsingSuccess = int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }

        private short ParseShort(string text)
        {
            short editValue;
            var parsingSuccess = short.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out editValue);

            if (parsingSuccess)
                return editValue;

            return 0;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            TargetType = values[1] as Type;

            return ConvertNumberToEditValueText(values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {ConvertBack(value)};
        }

        private string ConvertNumberToEditValueText(object editValue)
        {
            if (editValue is decimal)
                return ((decimal)editValue).ToString("n", GetFormatter());
            if (editValue is double)
                return ((double)editValue).ToString("n", GetFormatter());
            if (editValue is int)
                return ((int)editValue).ToString();
            if (editValue is short)
                return ((short)editValue).ToString();
            if (editValue is long)
                return ((long)editValue).ToString();

            throw new InvalidOperationException();
        }

        private NumberFormatInfo GetFormatter()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalSeparator = ".";
            return nfi;
        }

    }
}
