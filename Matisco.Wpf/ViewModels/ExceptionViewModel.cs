using System;
using System.Windows;
using System.Windows.Input;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf.ViewModels
{
    public class ExceptionViewModel : BindableBase, INavigationAware, IControlWindowProperties
    {
        public const string NavigationParameterException = "Exception";

        private readonly IApplicationShutdownService _applicationShutdownService;
        private readonly IWindowService _windowService;
        private string _message;
        private string _stackTrace;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public string StackTrace
        {
            get { return _stackTrace; }
            set
            {
                _stackTrace = value; 
                OnPropertyChanged();
            }
        }

        public ICommand ContinueCommand => new DelegateCommand(Continue);

        public ICommand ShutDownCommand => new DelegateCommand(ShutDown);

        public ExceptionViewModel(IApplicationShutdownService applicationShutdownService, IWindowService windowService)
        {
            _applicationShutdownService = applicationShutdownService;
            _windowService = windowService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var exception = navigationContext.Parameters[NavigationParameterException] as Exception;

            Message = exception?.Message;
            StackTrace = exception?.ToString();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var exception = navigationContext.Parameters[NavigationParameterException] as Exception;
            return exception != null;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void Continue()
        {
            _windowService.CloseContainingWindow(this);
        }

        private void ShutDown()
        {
            _applicationShutdownService.ExitApplication(true);
        }

        public WindowPropertyOverrides GetWindowPropertyOverrides()
        {
            return new WindowPropertyOverrides()
            {
                Title = "Error",
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
        }
    }
}
