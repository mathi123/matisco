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
using Matisco.Wpf.Controls.Coverters;

namespace Matisco.Wpf.Controls.Editors
{
    public class NumberEditor : Control
    {
        public const string PartReadOnlyTextBox = "PART_TextBoxReadOnly";
        public const string PartEditValueTextBox = "PART_TextBox";

        private TextBox _readOnlyTextBox;
        private TextBox _textBox;

        public static readonly DependencyProperty ShowRequiredIndicatorProperty = DependencyProperty.Register(
            "ShowRequiredIndicator", typeof(bool), typeof(NumberEditor), new PropertyMetadata(default(bool)));

        public bool ShowRequiredIndicator
        {
            get { return (bool)GetValue(ShowRequiredIndicatorProperty); }
            set { SetValue(ShowRequiredIndicatorProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(NumberEditor), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelSizeProperty = DependencyProperty.Register(
            "LabelSize", typeof(EditorSize), typeof(NumberEditor), new PropertyMetadata(EditorSize.Small));

        public EditorSize LabelSize
        {
            get { return (EditorSize)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(EditorSize), typeof(NumberEditor), new PropertyMetadata(EditorSize.Medium));

        public EditorSize Size
        {
            get { return (EditorSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public object EditValue
        {
            get { return (object)GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        public static readonly DependencyProperty EditValueProperty =
            DependencyProperty.Register("EditValue", typeof(object), typeof(NumberEditor), new PropertyMetadata(null, EditValueChanged));
        
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(NumberEditor), new PropertyMetadata(default(bool), ReadOnlyChanged));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.Register("Prefix", typeof(string), typeof(NumberEditor), new PropertyMetadata(""));
        
        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(NumberEditor), new PropertyMetadata(""));
        
        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        public static readonly DependencyProperty NumberGroupSeparatorProperty =
            DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(NumberEditor), new PropertyMetadata(""));

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }

        public static readonly DependencyProperty NumberDecimalSeparatorProperty =
         DependencyProperty.Register("NumberDecimalSeparator", typeof(string), typeof(NumberEditor), new PropertyMetadata(","));


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
            DependencyProperty.Register("Round", typeof(int), typeof(NumberEditor), new PropertyMetadata(Int32.MaxValue));

        public int RoundReadOnly
        {
            get { return (int)GetValue(RoundReadOnlyProperty); }
            set { SetValue(RoundReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty RoundReadOnlyProperty =
            DependencyProperty.Register("RoundReadOnly", typeof(int), typeof(NumberEditor), new PropertyMetadata(Int32.MaxValue));

        public Type EditValueType { get; set; }

        private static void ReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as NumberEditor;
            editor?.CheckEditMode();
        }

        private static void EditValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var editor = dependencyObject as NumberEditor;
            editor?.SetEditorType(dependencyPropertyChangedEventArgs.NewValue.GetType());
        }

        private void SetEditorType(Type editorType)
        {
            EditValueType = editorType;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBox = GetTemplateChild(PartEditValueTextBox) as TextBox;
            _readOnlyTextBox = GetTemplateChild(PartReadOnlyTextBox) as TextBox;

            _readOnlyTextBox.GotFocus += ReadOnlyTextBoxGotFocus;
            _textBox.LostKeyboardFocus += TextBoxLostKeyboardFocus;
            _textBox.PreviewTextInput += TextBoxPreviewTextInput;

        }

        public void CheckEditMode()
        {
            if (!IsInitialized)
                return;

            if (IsReadOnly)
            {
                if (_textBox != null && _textBox.IsKeyboardFocused)
                {
                    GoToEditModus(false);
                }
            }
            else
            {
                if (_readOnlyTextBox != null && _readOnlyTextBox.IsKeyboardFocused)
                {
                    GoToEditModus(true);
                }
            }
        }

        private void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach(var character in e.Text)
            {
                if (EditValueType == typeof(short) || EditValueType == typeof(int) || EditValueType == typeof(long))
                {
                    if (!char.IsNumber(character))
                    {
                        e.Handled = true;
                    }
                }
                else if(!char.IsNumber(character) && (character != '.' || _textBox.Text.Contains(".") || Round == 0))
                {
                    e.Handled = true;
                }
                else
                {
                    var possibleNewText = _textBox.Text + e.Text;

                    if (possibleNewText.Contains('.'))
                    {
                        if (possibleNewText.Length - possibleNewText.IndexOf('.') > Round + 1)
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
            _textBox.CaretIndex = int.MaxValue;
            _textBox.Focus();
            Keyboard.Focus(_textBox);
        }

        static NumberEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberEditor), new FrameworkPropertyMetadata(typeof(NumberEditor)));
        }
    }
}
