
using System.Windows.Input;
using Matisco.SomeApplication.CustomerManagement;
using Matisco.Wpf;
using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.SomeApplication
{
    public class StartUpViewModel : BindableBase, INavigationAware, INeedsRegionManager
    {
        private readonly IWindowService _windowService;
        private readonly IApplicationShutdownService _applicationShutdownService;

        public IRegionManager RegionManager { get; set; }

        public ICommand OpenShellCommand => new DelegateCommand(OpenShell);

        public ICommand ExitApplicationCommand => new DelegateCommand(ExitApplication);

        public ICommand OpenCustomersCommand => new DelegateCommand(OpenCustomers);
        
        public StartUpViewModel(IWindowService windowService, IApplicationShutdownService applicationShutdownService)
        {
            _windowService = windowService;
            _applicationShutdownService = applicationShutdownService;
        }

        private void OpenShell()
        {
            _windowService.OpenNewWindow<MyCustomShellView>();
        }

        private void ExitApplication()
        {
            _applicationShutdownService.ExitApplication(false);
        }

        private void OpenCustomers()
        {
            _windowService.Open(nameof(CustomerOverview));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

    }
}
