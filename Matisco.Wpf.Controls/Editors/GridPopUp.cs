using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Matisco.Wpf.Controls.Editors
{
    internal class GridPopUp : Control, IHasFocusElement
    {
        private const string PartButtonClear = "PART_ButtonClear";
        private const string PartButtonUp = "PART_ButtonUp";
        private const string PartTextBox = "PART_TextBox";
        private const string PartBorder = "PART_Border";
        private const string PartDataGrid= "PART_DataGrid";

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(IEnumerable), typeof(GridPopUp), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable Items
        {
            get { return (IEnumerable) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(GridPopUp), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get { return (object) GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty HightlightedItemProperty = DependencyProperty.Register(
            "HightlightedItem", typeof(object), typeof(GridPopUp), new PropertyMetadata(default(object)));

        public object HightlightedItem
        {
            get { return (object) GetValue(HightlightedItemProperty); }
            set { SetValue(HightlightedItemProperty, value); }
        }

        public static readonly DependencyProperty RecordsCountProperty = DependencyProperty.Register(
            "RecordsCount", typeof(int), typeof(GridPopUp), new PropertyMetadata(default(int)));

        public int RecordsCount
        {
            get { return (int) GetValue(RecordsCountProperty); }
            set { SetValue(RecordsCountProperty, value); }
        }

        public static readonly DependencyProperty ShowRequiredIndicatorProperty = DependencyProperty.Register(
            "ShowRequiredIndicator", typeof(bool), typeof(GridPopUp), new PropertyMetadata(default(bool)));

        public bool ShowRequiredIndicator
        {
            get { return (bool) GetValue(ShowRequiredIndicatorProperty); }
            set { SetValue(ShowRequiredIndicatorProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(EditorSize), typeof(GridPopUp), new PropertyMetadata(EditorSize.Medium));

        public EditorSize Size
        {
            get { return (EditorSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(
            "Query", typeof(string), typeof(GridPopUp), new PropertyMetadata(default(string)));

        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            "DisplayMemberPath", typeof(string), typeof(GridPopUp), new PropertyMetadata(default(string)));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        private TextBox _textBox;
        private DataGrid _dataGrid;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var border = GetTemplateChild(PartButtonUp) as Button;
            _textBox = GetTemplateChild(PartTextBox) as TextBox;
            _dataGrid = GetTemplateChild(PartDataGrid) as DataGrid;

            border.Click += OnButtonClick;
            _textBox.KeyUp += TextBoxOnKeyUp;

            _dataGrid.MouseUp += DataGridOnMouseUp;
        }

        private void TextBoxOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Down || keyEventArgs.Key == Key.Enter)
            {
                _dataGrid.Focus();
                Keyboard.Focus(_dataGrid);
            }
            else if (keyEventArgs.Key == Key.Escape || keyEventArgs.Key == Key.Tab)
            {
                CloseWindow();
            }
        }

        private void DataGridOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (HightlightedItem != null)
            {
                SelectedItem = HightlightedItem;
                CloseWindow();
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            (Parent as Window).Close();
        }

        static GridPopUp()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridPopUp), new FrameworkPropertyMetadata(typeof(GridPopUp)));
        }

        public IInputElement GetFocusElement()
        {
            return _textBox;
        }
    }

    internal interface IHasFocusElement
    {
        IInputElement GetFocusElement();
    }
}
