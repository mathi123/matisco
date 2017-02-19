using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf.ViewModels
{
    public class ModalViewModel : BindableBase, INavigationAware, IControlWindowProperties, IHasResults
    {
        private readonly IWindowService _windowService;
        public event WindowPropertiesChangedDelegate WindowPropertiesChanged;

        private string _iconPath;
        private bool _hasYesButton;
        private bool _hasNoButton;
        private bool _hasOkButton;
        private bool _hasCancelButton;
        private string _title;
        private string _message;
        private string _imagePath;
        private bool _hasCustomIcon;
        private ModalButtonEnum? _windowResult;

        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                _iconPath = value; 
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value; 
                OnPropertyChanged();
            }
        }

        public bool HasCustomIcon
        {
            get { return _hasCustomIcon; }
            set
            {
                _hasCustomIcon = value; 
                OnPropertyChanged();
            }
        }

        public bool HasYesButton
        {
            get { return _hasYesButton; }
            set
            {
                _hasYesButton = value; 
                OnPropertyChanged();
            }
        }

        public bool HasNoButton
        {
            get { return _hasNoButton; }
            set
            {
                _hasNoButton = value; 
                OnPropertyChanged();
            }
        }

        public bool HasOkButton
        {
            get { return _hasOkButton; }
            set
            {
                _hasOkButton = value; 
                OnPropertyChanged();
            }
        }

        public bool HasCancelButton
        {
            get { return _hasCancelButton; }
            set
            {
                _hasCancelButton = value; 
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value; 
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand => new DelegateCommand<ModalButtonEnum?>(Close);

        public ModalViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = navigationContext.Parameters[nameof(Title)] as string;
            Message = navigationContext.Parameters[nameof(Message)] as string;

            var modalIcon = (ModalIconEnum)navigationContext.Parameters[nameof(ModalIconEnum)];
            HasCustomIcon = modalIcon != ModalIconEnum.None;

            if (HasCustomIcon)
            {
                IconPath = GetIconPath(modalIcon, "ico");
                ImagePath = GetIconPath(modalIcon, "png");
            }

            var modalButtons = (ModalButtonEnum[]) navigationContext.Parameters[nameof(ModalButtonEnum)];
            HasYesButton = modalButtons.Contains(ModalButtonEnum.Yes);
            HasNoButton = modalButtons.Contains(ModalButtonEnum.No);
            HasOkButton = modalButtons.Contains(ModalButtonEnum.Ok);
            HasCancelButton = modalButtons.Contains(ModalButtonEnum.Cancel);

            WindowPropertiesChanged?.Invoke(this);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public WindowPropertyOverrides GetWindowPropertyOverrides()
        {
            return new WindowPropertyOverrides()
            {
                Title = Title,
                IconPath = HasCustomIcon ? IconPath : null,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight
            };
        }

        public object[] GetResults()
        {
            return new object[] { _windowResult };
        }

        private string GetIconPath(ModalIconEnum modalIcon, string suffix)
        {
            const string basePath = "pack://application:,,,/Matisco.Wpf;component/Resources/{0}.{1}";
            string iconPath = "";

            switch (modalIcon)
            {
                case ModalIconEnum.None:
                    break;
                case ModalIconEnum.Question:
                    iconPath = "DialogQuestion";
                    break;
                case ModalIconEnum.Information:
                    iconPath = "DialogInfo";
                    break;
                case ModalIconEnum.Warning:
                    iconPath = "DialogWarning";
                    break;
                case ModalIconEnum.Error:
                    iconPath = "DialogError";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return string.Format(basePath, iconPath, suffix);
        }

        private void Close(ModalButtonEnum? result)
        {
            _windowResult = result;
            _windowService.CloseContainingWindow(this);
        }
    }
}
