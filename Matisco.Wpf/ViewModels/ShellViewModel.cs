using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IWindowService _windowService;

        private readonly WindowPropertyOverrides _originalWindowState = new WindowPropertyOverrides()
        {
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.CanResize,
            ShowInTaskbar = true,
            WindowStyle = WindowStyle.SingleBorderWindow,
            WindowState = WindowState.Normal,
            CloseOnDeactivate = false,
            ExitApplicationOnClose = false
        };

        private string _title;
        private ImageSource _icon;
        private SizeToContent _sizeToContent;
        private ResizeMode _resizeMode;
        private bool _showInTaskbar;
        private WindowStyle _windowStyle;
        private WindowState _windowState;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                RaisePropertyChanged();
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value; 
                RaisePropertyChanged();
            }
        }
        
        public SizeToContent SizeToContent
        {
            get { return _sizeToContent; }
            set
            {
                _sizeToContent = value;
                RaisePropertyChanged();
            }
        }

        public ResizeMode ResizeMode
        {
            get { return _resizeMode; }
            set
            {
                _resizeMode = value; 
                RaisePropertyChanged();
            }
        }

        public bool ShowInTaskbar
        {
            get { return _showInTaskbar; }
            set
            {
                _showInTaskbar = value; 
                RaisePropertyChanged();
            }
        }

        public WindowStyle WindowStyle
        {
            get { return _windowStyle; }
            set
            {
                _windowStyle = value; 
                RaisePropertyChanged();
            }
        }

        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value; 
                RaisePropertyChanged();
            }
        }

        public bool CloseOnDeactivate { get; set; }

        public bool ExitApplicationOnClose { get; set; }

        public ICommand CloseShellCommand => new DelegateCommand(CloseShell);

        public ShellViewModel(IShellInformationService shellInformationService, IRegionManager regionManager, IWindowService windowService)
        {
            _windowService = windowService;
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

            if (windowProps.WindowState.HasValue)
            {
                WindowState = windowProps.WindowState.Value;

                if (WindowState == WindowState.Maximized)
                {
                    SizeToContent = SizeToContent.Manual;
                }
            }

            ExitApplicationOnClose = windowProps.ExitApplicationOnClose;
        }

        private void CloseShell()
        {
            _windowService.CloseContainingWindow(this);   
        }
    }
}
