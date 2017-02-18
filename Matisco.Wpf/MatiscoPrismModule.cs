using Matisco.Wpf.Services;
using Matisco.Wpf.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Matisco.Wpf
{
    public class MatiscoPrismModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IShellInformationService _shellInformationService;

        public MatiscoPrismModule(IRegionManager regionManager, IShellInformationService shellInformationService)
        {
            _regionManager = regionManager;
            _shellInformationService = shellInformationService;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(UnsavedChangesView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ExceptionView));

            _shellInformationService.SetShellType(typeof(ShellView));
        }
    }
}
