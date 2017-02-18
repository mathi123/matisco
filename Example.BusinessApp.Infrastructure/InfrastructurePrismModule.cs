using Matisco.Wpf;
using Prism.Modularity;
using Prism.Regions;

namespace Example.BusinessApp.Infrastructure
{
    public class InfrastructurePrismModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public InfrastructurePrismModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(Views.StartUpView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(Views.ConfirmView));
        }
    }
}
