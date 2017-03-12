using System;
using System.Globalization;
using System.Windows.Data;

namespace Matisco.Wpf.Controls.Coverters
{
    internal class TextToNumberConverter : IValueConverter
    {
        public Type TargetType { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
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

            return 0;
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
    }
}
