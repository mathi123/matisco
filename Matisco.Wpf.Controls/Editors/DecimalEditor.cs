using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        public const string PartReadOnlyTextBox = "PART_TextBoxReadOnly";
        public const string PartEditValueTextBox = "PART_TextBox";

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

        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.Register("Prefix", typeof(string), typeof(DecimalEditor), new PropertyMetadata(""));
        
        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(DecimalEditor), new PropertyMetadata(""));
        
        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSeparatorProperty =
            DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(DecimalEditor), new PropertyMetadata("."));

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }

        public static readonly DependencyProperty NumberDecimalSeparatorProperty =
         DependencyProperty.Register("NumberDecimalSeparator", typeof(string), typeof(DecimalEditor), new PropertyMetadata(","));


        public string NumberDecimalSeparator
        {
            get { return (string)GetValue(NumberDecimalSeparatorProperty); }
            set { SetValue(NumberDecimalSeparatorProperty, value); }
        }
        
        public int Round
        {
            get { return (int)GetValue(RoundProperty); }
            set { SetValue(RoundProperty, value); }
        }

        public static readonly DependencyProperty RoundProperty =
            DependencyProperty.Register("Round", typeof(int), typeof(DecimalEditor), new PropertyMetadata(Int32.MaxValue));

        public int RoundReadOnly
        {
            get { return (int)GetValue(RoundReadOnlyProperty); }
            set { SetValue(RoundReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty RoundReadOnlyProperty =
            DependencyProperty.Register("RoundReadOnly", typeof(int), typeof(DecimalEditor), new PropertyMetadata(Int32.MaxValue));

        private TextBox _readOnlyTextBox;
        private TextBox _textBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _readOnlyTextBox = GetTemplateChild(PartReadOnlyTextBox) as TextBox;
            _textBox = GetTemplateChild(PartEditValueTextBox) as TextBox;

            _readOnlyTextBox.GotFocus += ReadOnlyTextBoxGotFocus;
            _textBox.LostKeyboardFocus += TextBoxLostKeyboardFocus;
            _textBox.PreviewTextInput += TextBoxPreviewTextInput;
        }

        private void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach(var character in e.Text)
            {
                if(!char.IsNumber(character) && (character != '.' || e.Text.Contains(".") || Round == 0))
                {
                    e.Handled = true;
                }
                else
                {
                    var possibleNewText = _textBox.Text + e.Text;

                    if (possibleNewText.Contains('.'))
                    {
                        if(possibleNewText.Length - possibleNewText.IndexOf('.') > Round + 1)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void TextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GoToEditModus(false);
        }

        private void ReadOnlyTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if(!IsReadOnly)
            {
                GoToEditModus(true);
            }
        }

        private void GoToEditModus(bool editModus)
        {
            _readOnlyTextBox.Visibility = editModus ? Visibility.Hidden : Visibility.Visible;
            _textBox.Visibility = editModus ? Visibility.Visible : Visibility.Hidden;
            _textBox.Text = ConvertDecimalToEditValueText(EditValue);
            _textBox.CaretIndex = int.MaxValue;
            _textBox.Focus();
            Keyboard.Focus(_textBox);
        }

        private string ConvertDecimalToEditValueText(decimal editValue)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalSeparator = ".";

            var decimalText = editValue.ToString("n", nfi);

            return decimalText;
        }

        static DecimalEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalEditor), new FrameworkPropertyMetadata(typeof(DecimalEditor)));
        }
    }
}
