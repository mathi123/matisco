using Example.BusinessApp.Sales.Views;
using Matisco.Wpf;
using Prism.Modularity;
using Prism.Regions;

namespace Example.BusinessApp.Sales
{
    public class SalesPrismModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SalesPrismModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(CustomerOverview));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(CustomerView));
        }
    }
}
