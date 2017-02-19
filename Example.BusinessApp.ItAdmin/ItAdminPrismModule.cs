using Example.BusinessApp.ItAdmin.Views;
using Matisco.Wpf;
using Prism.Modularity;
using Prism.Regions;

namespace Example.BusinessApp.ItAdmin
{
    public class ItAdminPrismModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ItAdminPrismModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ModalSamplesView));
        }
    }
}
