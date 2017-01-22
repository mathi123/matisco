using System.Collections.ObjectModel;
using System.Windows.Input;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Matisco.Wpf.ViewModels
{
    public class UnsavedChangesViewModel : BindableBase
    {
        private readonly IApplicationShutdownService _applicationShutdownService;
        private readonly IWindowService _windowService;
        private ObservableCollection<string> _windowTitles;

        public ObservableCollection<string> WindowTitles
        {
            get { return _windowTitles; }
            set
            {
                _windowTitles = value; 
                OnPropertyChanged();
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
    }
}
