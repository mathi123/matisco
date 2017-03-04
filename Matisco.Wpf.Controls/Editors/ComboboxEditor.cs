using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Matisco.Wpf.Controls.Editors
{
    public class ComboboxEditor : Control
    {
        private const string PartButtonClear = "PART_ButtonClear";
        private const string PartButtonDown = "PART_ButtonDown";
        private const string PartTextBox = "PART_TextBox";
        private const string PartBorder = "PART_Border";

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register(
            "EditValue", typeof(object), typeof(ComboboxEditor), new PropertyMetadata(default(object)));

        public object EditValue
        {
            get { return (object) GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(ComboboxEditor), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
       
        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(
            "Query", typeof(string), typeof(ComboboxEditor), new PropertyMetadata(default(string)));

        public string Query
        {
            get { return (string) GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(ObservableCollection<DataGridColumn>), typeof(ComboboxEditor), new PropertyMetadata(default(ObservableCollection<DataGridColumn>)));

        public ObservableCollection<DataGridColumn> Columns
        {
            get { return (ObservableCollection<DataGridColumn>) GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(ComboboxEditor), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelSizeProperty = DependencyProperty.Register(
            "LabelSize", typeof(EditorSize), typeof(ComboboxEditor), new PropertyMetadata(default(EditorSize)));

        public EditorSize LabelSize
        {
            get { return (EditorSize) GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(EditorSize), typeof(ComboboxEditor), new PropertyMetadata(EditorSize.Medium));

        public EditorSize Size
        {
            get { return (EditorSize) GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty ShowRequiredIndicatorProperty = DependencyProperty.Register(
            "ShowRequiredIndicator", typeof(bool), typeof(ComboboxEditor), new PropertyMetadata(default(bool)));

        private TextBox _textBox;
        private Border _border;

        public bool ShowRequiredIndicator
        {
            get { return (bool) GetValue(ShowRequiredIndicatorProperty); }
            set { SetValue(ShowRequiredIndicatorProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            "DisplayMemberPath", typeof(string), typeof(ComboboxEditor), new PropertyMetadata(default(string)));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var downButton = GetTemplateChild(PartButtonDown) as Button;
            var clearButton = GetTemplateChild(PartButtonClear) as Button;
            _border = GetTemplateChild(PartBorder) as Border;
            _textBox = GetTemplateChild(PartTextBox) as TextBox;

            downButton.Click += ArrowClicked;
            clearButton.Click += ClearClicked;
            _textBox.GotKeyboardFocus += TextBoxGotFocus;
            _textBox.KeyUp += TextBoxKeyUp;
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                
            }
        }

        private void TextBoxGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!_windowJustClosed)
            {
                OpenGridPopUp();
            }
            else
            {
                _windowJustClosed = false;
            }
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Clear clicked");
        }

        private void ArrowClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Arrow clicked");
            OpenGridPopUp();
        }
        
        private void OpenGridPopUp()
        {
            var locationFromScreen = _border.PointToScreen(new Point(0, 0));
            var source = PresentationSource.FromVisual(_border);
            var targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);

            var control = new GridPopUp();
            control.Items = ItemsSource;
            control.ShowRequiredIndicator = ShowRequiredIndicator;
            control.DisplayMemberPath = DisplayMemberPath;
            control.SelectedItem = EditValue;

            var window = new PopUpWindow();
            window.Top = targetPoints.Y;
            window.Left = targetPoints.X;
            window.Content = control;

            window.Closed += Window_Closed;

            window.Show();
        }

        private bool _windowJustClosed = false;
        private void Window_Closed(object sender, EventArgs e)
        {
            _windowJustClosed = true;
        }

        static ComboboxEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboboxEditor), new FrameworkPropertyMetadata(typeof(ComboboxEditor)));
        }
    }
}
