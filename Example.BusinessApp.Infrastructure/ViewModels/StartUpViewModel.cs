using System.Windows.Input;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.Infrastructure.ViewModels
{
    public class StartUpViewModel : BindableBase, INavigationAware//, INeedsRegionManager
    {
        private readonly IWindowService _windowService;
        private readonly IApplicationShutdownService _applicationShutdownService;

        //public IRegionManager RegionManager { get; set; }

        public ICommand ExitApplicationCommand => new DelegateCommand(ExitApplication);

        public ICommand OpenCustomersCommand => new DelegateCommand(OpenCustomers);

        public ICommand OpenModalSamplesCommand => new DelegateCommand(OpenModalSamples);
        
        public StartUpViewModel(IWindowService windowService, IApplicationShutdownService applicationShutdownService)
        {
            _windowService = windowService;
            _applicationShutdownService = applicationShutdownService;
        }
        
        private void ExitApplication()
        {
            _applicationShutdownService.ExitApplication(false);
        }

        private void OpenCustomers()
        {
            _windowService.Open("UserOverview");
        }

        private void OpenModalSamples()
        {
            _windowService.Open(ViewNames.ModalSamplesView);
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
