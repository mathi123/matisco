using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Matisco.Wpf.Controls.Editors
{
    public class DecimalEditor : Control
    {
        public static readonly DependencyProperty ShowRequiredIndicatorProperty = DependencyProperty.Register(
            "ShowRequiredIndicator", typeof(bool), typeof(DecimalEditor), new PropertyMetadata(default(bool)));

        public bool ShowRequiredIndicator
        {
            get { return (bool)GetValue(ShowRequiredIndicatorProperty); }
            set { SetValue(ShowRequiredIndicatorProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(DecimalEditor), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelSizeProperty = DependencyProperty.Register(
            "LabelSize", typeof(EditorSize), typeof(DecimalEditor), new PropertyMetadata(EditorSize.Small));

        public EditorSize LabelSize
        {
            get { return (EditorSize)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(EditorSize), typeof(DecimalEditor), new PropertyMetadata(EditorSize.Medium));

        public EditorSize Size
        {
            get { return (EditorSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register(
            "EditValue", typeof(decimal), typeof(DecimalEditor), new PropertyMetadata(default(decimal)));

        public decimal EditValue
        {
            get { return (decimal)GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(DecimalEditor), new PropertyMetadata(default(bool)));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        static DecimalEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalEditor), new FrameworkPropertyMetadata(typeof(DecimalEditor)));
        }
    }
}
