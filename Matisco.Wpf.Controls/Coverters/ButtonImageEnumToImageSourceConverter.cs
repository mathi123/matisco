using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Matisco.Wpf.Controls.Buttons;

namespace Matisco.Wpf.Controls.Coverters
{
    public class ButtonImageEnumToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = (ButtonImageEnum) value;

            if (image == ButtonImageEnum.None)
                return null;

            return new BitmapImage(new Uri($"/Matisco.Wpf.Controls;component/Images/48/{image}.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
