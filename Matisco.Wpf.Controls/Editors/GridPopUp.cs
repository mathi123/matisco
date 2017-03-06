using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private const string PartTextBoxQuery = "PART_TextBoxQuery";
        private const string PartEditValueBorder = "PART_EditValueBorder";

        private TextBox _textBox;
        private DataGrid _dataGrid;
        private TextBox _queryBox;

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


        public static readonly DependencyProperty ShowSearchProperty = DependencyProperty.Register(
            "ShowSearch", typeof(bool), typeof(GridPopUp), new PropertyMetadata(true));

        public bool ShowSearch
        {
            get { return (bool)GetValue(ShowSearchProperty); }
            set { SetValue(ShowSearchProperty, value); }
        }

        public static readonly DependencyProperty ShowTotalCountProperty = DependencyProperty.Register(
            "ShowTotalCount", typeof(bool), typeof(GridPopUp), new PropertyMetadata(true));

        public bool ShowTotalCount
        {
            get { return (bool)GetValue(ShowTotalCountProperty); }
            set { SetValue(ShowTotalCountProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            "DataSource", typeof(AsynDataLoader), typeof(GridPopUp), new PropertyMetadata(default(AsynDataLoader)));

        public AsynDataLoader DataSource
        {
            get { return (AsynDataLoader)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            "IsLoading", typeof(bool), typeof(GridPopUp), new PropertyMetadata(default(bool)));

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var border = GetTemplateChild(PartButtonUp) as Button;
            var clearButton = GetTemplateChild(PartButtonClear) as Button;
            var selectedItemBorder = GetTemplateChild(PartEditValueBorder) as Border;
            _textBox = GetTemplateChild(PartTextBox) as TextBox;
            _dataGrid = GetTemplateChild(PartDataGrid) as DataGrid;
            _queryBox = GetTemplateChild(PartTextBoxQuery) as TextBox;

            border.Click += OnButtonClick;
            _textBox.MouseLeftButtonUp += SelectedItemClicked;
            _dataGrid.MouseUp += DataGridOnMouseUp;
            _dataGrid.PreviewKeyUp += DatagridKeyUp;

            _queryBox.KeyUp += QueryBoxKeyUp;
            _queryBox.TextChanged += QueryBoxOnTextChanged;
            clearButton.Click += ClearButton_Click;
        //    selectedItemBorder.PreviewMouseLeftButtonUp += BorderOnPreviewMouseLeftButtonUp;
        //}

        //private void BorderOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    mouseButtonEventArgs.Handled = true;
        //    CloseWindow();
        }

        private void DatagridKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ConfirmHightligtedItem();
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                CloseWindow();
            }
            else if (e.Key == Key.Up)
            {
                if (HightlightedItem == Items.Cast<object>().FirstOrDefault())
                {
                    e.Handled = true;
                    // move focus back to textbox
                    _queryBox.Focus();
                    Keyboard.Focus(_queryBox);
                }
            }
        }

        private void QueryBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (ReferenceEquals(DataSource, null))
            {
                FilterItemsSource();
            }
            else
            {
                FireSearch();
            }
        }

        private IEnumerable _allData;
        private void FireSearch()
        {
        }

        private void FilterItemsSource()
        {
            if (ReferenceEquals(_allData, null))
            {
                _allData = Items;
            }

            if (string.IsNullOrEmpty(Query))
            {
                Items = _allData;
            }
            else
            {
                var list = new List<object>();
                foreach (var record in _allData)
                {
                    foreach (var propertyInfo in record.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
                    {
                        if (propertyInfo.GetValue(record).ToString().ToLower().Contains(Query.ToLower()))
                        {
                            list.Add(record);
                            break;
                        }
                    }
                }

                Items = list;
            }

            RecordsCount = Items.Cast<object>().Count();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearItem();
        }

        private void ClearItem()
        {
            SelectedItem = null;
        }

        private void SelectedItemClicked(object sender, MouseButtonEventArgs e)
        {
            CloseWindow();
        }

        private void QueryBoxKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Down || keyEventArgs.Key == Key.Enter)
            {
                var items = Items.Cast<object>();

                if (!items.Any())
                    return;

                if (HightlightedItem == null)
                {
                    HightlightedItem = items.FirstOrDefault();
                }

                var request = new TraversalRequest(FocusNavigationDirection.Next);

                var elementWithFocus = Keyboard.FocusedElement as UIElement;
                elementWithFocus?.MoveFocus(request);
            }
            else if (keyEventArgs.Key == Key.Escape || keyEventArgs.Key == Key.Up)
            {
                CloseWindow();
            }
        }

        private void DataGridOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (HightlightedItem != null)
            {
                ConfirmHightligtedItem();
            }
        }

        private void ConfirmHightligtedItem()
        {
            SelectedItem = HightlightedItem;
            CloseWindow();
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
            return _queryBox;
        }
    }

    internal interface IHasFocusElement
    {
        IInputElement GetFocusElement();
    }
}
