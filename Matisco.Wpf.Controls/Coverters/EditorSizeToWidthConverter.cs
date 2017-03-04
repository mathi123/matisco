using System;
using System.Globalization;
using System.Windows.Data;
using Matisco.Wpf.Controls.Editors;

namespace Matisco.Wpf.Controls.Coverters
{
    public class EditorSizeToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (EditorSize)value;

            switch (size)
            {
                case EditorSize.Small:
                    return 75;
                case EditorSize.Medium:
                    return 140;
                case EditorSize.Large:
                    return 280;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
