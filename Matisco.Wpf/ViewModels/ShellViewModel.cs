using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf.ViewModels
{
    public class ShellViewModel : BindableBase, INeedsRegionManager
    {
        private readonly IShellInformationService _shellInformationService;
        private string _title;
        private ImageSource _icon;
        private IRegionManager _regionManager;
        private SizeToContent _sizeToContent = SizeToContent.Manual;
        private ResizeMode _resizeMode = ResizeMode.CanResize;

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

        public IRegionManager RegionManager
        {
            get { return _regionManager; }
            set
            {
                _regionManager = value;

                foreach (var region in _regionManager.Regions)
                {
                    region.ActiveViews.CollectionChanged += ViewChanged;
                }
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

        private void ViewChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var view = e.NewItems[0] as IHasTitle;

                if (view != null)
                {
                    Title = view.GetTitle();
                }
                else
                {
                    var dataContext = (e.NewItems[0] as FrameworkElement)?.DataContext as IHasTitle;

                    if (dataContext != null)
                    {
                        Title = dataContext.GetTitle();
                    }

                    var windowPropertyController = (e.NewItems[0] as FrameworkElement)?.DataContext as IControlWindowProperties;

                    if (windowPropertyController != null)
                    {
                        SetWindowProperties(windowPropertyController.GetWindowPropertyOverrides());
                    }
                }
            }
            else
            {
                Title = _shellInformationService.GetDefaultTitle();
            }
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
        }

        public ShellViewModel(IShellInformationService shellInformationService)
        {
            _shellInformationService = shellInformationService;

            Title = shellInformationService.GetDefaultTitle();
            var iconPath = shellInformationService.GetDefaultIconPath();
            
            Icon = new ImageSourceConverter().ConvertFrom(iconPath) as ImageSource;
        }
    }
}
