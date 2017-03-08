using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Matisco.Wpf.Controls.Coverters;

namespace Matisco.Wpf.Controls.Editors
{
    public class ComboboxEditor : Control
    {
        private const string PartButtonClear = "PART_ButtonClear";
        private const string PartButtonDown = "PART_ButtonDown";
        private const string PartTextBox = "PART_TextBox";
        private const string PartBorder = "PART_Border";
        private const string PartEditValueBorder = "PART_EditValueBorder";

        private TextBox _textBox;
        private Border _border;

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

        public static readonly DependencyProperty ShowSearchProperty = DependencyProperty.Register(
            "ShowSearch", typeof(bool), typeof(ComboboxEditor), new PropertyMetadata(true));

        public bool ShowSearch
        {
            get { return (bool) GetValue(ShowSearchProperty); }
            set { SetValue(ShowSearchProperty, value); }
        }

        public static readonly DependencyProperty ShowTotalCountProperty = DependencyProperty.Register(
            "ShowTotalCount", typeof(bool), typeof(ComboboxEditor), new PropertyMetadata(true));

        public bool ShowTotalCount
        {
            get { return (bool) GetValue(ShowTotalCountProperty); }
            set { SetValue(ShowTotalCountProperty, value); }
        }

        public static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register(
            "TotalCount", typeof(int), typeof(ComboboxEditor), new PropertyMetadata(default(int)));

        public int TotalCount
        {
            get { return (int) GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            "DataSource", typeof(AsynDataLoader), typeof(ComboboxEditor), new PropertyMetadata(default(AsynDataLoader)));

        public AsynDataLoader DataSource
        {
            get { return (AsynDataLoader) GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(ComboboxEditor), new PropertyMetadata(default(bool)));

        public bool IsReadOnly
        {
            get { return (bool) GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var downButton = GetTemplateChild(PartButtonDown) as Button;
            var clearButton = GetTemplateChild(PartButtonClear) as Button;
            var border = GetTemplateChild(PartEditValueBorder) as Border;
            _border = GetTemplateChild(PartBorder) as Border;
            _textBox = GetTemplateChild(PartTextBox) as TextBox;

            downButton.Click += ArrowClicked;
            clearButton.Click += ClearClicked;
            _textBox.GotKeyboardFocus += TextBoxGotFocus;
            _textBox.MouseLeftButtonUp += TextboxMouseLeftButtonUp;
            _textBox.KeyUp += TextBoxKeyUp;
        //    border.PreviewMouseLeftButtonUp += BorderOnPreviewMouseLeftButtonUp;
        //}

        //private void BorderOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    mouseButtonEventArgs.Handled = true;
        //    OpenGridPopUp();
        }
        
        private void TextboxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenGridPopUp();
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Enter || e.Key == Key.Down)
            {
                OpenGridPopUp();
            }
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                ClearValue();
            }
        }

        private void TextBoxGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            ClearValue();
        }

        private void ClearValue()
        {
            EditValue = null;
        }

        private void ArrowClicked(object sender, RoutedEventArgs e)
        {
            OpenGridPopUp();
        }
        
        private void OpenGridPopUp()
        {
            var locationFromScreen = _border.PointToScreen(new Point(0, 0));
            var source = PresentationSource.FromVisual(_border);
            var targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);

            var control = new GridPopUp();
            SetProperties(control);

            var window = new PopUpWindow();
            window.Top = targetPoints.Y;
            window.Left = targetPoints.X;
            window.Content = control;
            window.Closed += PopUpClosed;
            window.Show();
        }

        private void SetProperties(GridPopUp control)
        {
            control.Items = ItemsSource;
            control.ShowRequiredIndicator = ShowRequiredIndicator;
            control.DisplayMemberPath = DisplayMemberPath;
            control.SelectedItem = EditValue;
            control.ShowSearch = ShowSearch;
            control.ShowTotalCount = ShowTotalCount;
            control.DataSource = DataSource;
            control.IsReadOnly = IsReadOnly;
        }

        private void PopUpClosed(object sender, EventArgs e)
        {
            var control = (sender as PopUpWindow)?.Content as GridPopUp;

            if (control != null && !IsReadOnly)
            {
                EditValue = control.SelectedItem;
            }
        }

        static ComboboxEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboboxEditor), new FrameworkPropertyMetadata(typeof(ComboboxEditor)));
        }
    }

    public interface AsynDataLoader
    {
        Task<IEnumerable<object>> LoadData(string searchQuery, int skip, int take);
    }
}
