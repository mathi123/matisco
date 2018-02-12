using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Matisco.Wpf.ViewModels
{
    public class UnsavedChangesViewModel : BindableBase, IControlWindowProperties
    {
        public event WindowPropertiesChangedDelegate WindowPropertiesChanged;

        private readonly IApplicationShutdownService _applicationShutdownService;
        private readonly IWindowService _windowService;
        private ObservableCollection<string> _windowTitles;
        private string _message = "There are unsaved changes.";

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value; 
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> WindowTitles
        {
            get { return _windowTitles; }
            set
            {
                _windowTitles = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand ShutDownCommand => new DelegateCommand(ShutDown);

        public ICommand CancelCommand => new DelegateCommand(Cancel);

        public UnsavedChangesViewModel(IApplicationShutdownService applicationShutdownService, IWindowService windowService)
        {
            _applicationShutdownService = applicationShutdownService;
            _windowService = windowService;
            var windows = _applicationShutdownService.GetBlockingWindows();
            WindowTitles = new ObservableCollection<string>(windows);
        }

        private void Cancel()
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
                Title = "Unsaved changes",
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                IconPath = "pack://application:,,,/Matisco.Wpf;component/Resources/DialogWarning.ico"
            };
        }
    }
}
