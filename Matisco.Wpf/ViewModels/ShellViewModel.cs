using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly WindowPropertyOverrides _originalWindowState = new WindowPropertyOverrides()
        {
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.CanResize,
            ShowInTaskbar = true,
            WindowStyle = WindowStyle.SingleBorderWindow
        };

        private string _title;
        private ImageSource _icon;
        private SizeToContent _sizeToContent;
        private ResizeMode _resizeMode;
        private bool _showInTaskbar;
        private WindowStyle _windowStyle;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value; 
                OnPropertyChanged();
            }
        }
        
        public SizeToContent SizeToContent
        {
            get { return _sizeToContent; }
            set
            {
                _sizeToContent = value;
                OnPropertyChanged();
            }
        }

        public ResizeMode ResizeMode
        {
            get { return _resizeMode; }
            set
            {
                _resizeMode = value; 
                OnPropertyChanged();
            }
        }

        public bool ShowInTaskbar
        {
            get { return _showInTaskbar; }
            set
            {
                _showInTaskbar = value; 
                OnPropertyChanged();
            }
        }

        public WindowStyle WindowStyle
        {
            get { return _windowStyle; }
            set
            {
                _windowStyle = value; 
                OnPropertyChanged();
            }
        }
        
        public bool CloseOnDeactivate { get; set; }

        public ShellViewModel(IShellInformationService shellInformationService, IRegionManager regionManager)
        {
            _originalWindowState.Title = shellInformationService.GetDefaultTitle();
            _originalWindowState.IconPath = shellInformationService.GetDefaultIconPath();

            SetWindowProperties(_originalWindowState);

            regionManager.Regions.CollectionChanged += RegionCollectionChanged;
        }

        private void RegionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var region in e.NewItems)
                {
                    var iRegion = region as IRegion;
                    if (iRegion != null) iRegion.ActiveViews.CollectionChanged += ViewChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var region in e.OldItems)
                {
                    var iRegion = region as IRegion;
                    if (iRegion != null) iRegion.ActiveViews.CollectionChanged -= ViewChanged;
                }
            }
        }

        private void ViewChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var view in e.NewItems)
                {
                    var windowPropertyController = (view as FrameworkElement)?.DataContext as IControlWindowProperties;

                    if (windowPropertyController != null)
                    {
                        windowPropertyController.WindowPropertiesChanged += WindowPropertyControllerOnWindowPropertiesChanged;

                        SetWindowProperties(windowPropertyController.GetWindowPropertyOverrides());
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var view in e.OldItems)
                {
                    var windowPropertyController = (view as FrameworkElement)?.DataContext as IControlWindowProperties;

                    if (windowPropertyController != null)
                    {
                        windowPropertyController.WindowPropertiesChanged -= WindowPropertyControllerOnWindowPropertiesChanged;

                        SetWindowProperties(_originalWindowState);
                    }
                }
            }

            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    var view = e.NewItems[0] as IHasTitle;

            //    if (view != null)
            //    {
            //        Title = view.GetTitle();
            //    }
            //    else
            //    {
            //        var dataContext = (e.NewItems[0] as FrameworkElement)?.DataContext as IHasTitle;

            //        if (dataContext != null)
            //        {
            //            Title = dataContext.GetTitle();
            //        }

            //        var windowPropertyController = (e.NewItems[0] as FrameworkElement)?.DataContext as IControlWindowProperties;

            //        if (windowPropertyController != null)
            //        {
            //            windowPropertyController.WindowPropertiesChanged +=
            //                (win) => SetWindowProperties(win.GetWindowPropertyOverrides());

            //            SetWindowProperties(windowPropertyController.GetWindowPropertyOverrides());
            //        }
            //    }
            //}
            //else
            //{
            //    Title = _shellInformationService.GetDefaultTitle();
            //}
        }

        private void WindowPropertyControllerOnWindowPropertiesChanged(IControlWindowProperties sender)
        {
            SetWindowProperties(sender.GetWindowPropertyOverrides());
        }

        private void SetWindowProperties(WindowPropertyOverrides windowProps)
        {
            if (windowProps.SizeToContent.HasValue)
            {
                SizeToContent = windowProps.SizeToContent.Value;
            }

            if (windowProps.ResizeMode.HasValue)
            {
                ResizeMode = windowProps.ResizeMode.Value;
            }

            if (windowProps.Title != null)
            {
                Title = windowProps.Title;
            }

            if (windowProps.IconPath != null)
            {
                Icon = new ImageSourceConverter().ConvertFrom(windowProps.IconPath) as ImageSource;
            }

            if (windowProps.ShowInTaskbar.HasValue)
            {
                ShowInTaskbar = windowProps.ShowInTaskbar.Value;
            }

            if (windowProps.WindowStyle.HasValue)
            {
                WindowStyle = windowProps.WindowStyle.Value;
            }

            if (windowProps.CloseOnDeactivate.HasValue)
            {
                CloseOnDeactivate = windowProps.CloseOnDeactivate.Value;
            }
        }
    }
}
