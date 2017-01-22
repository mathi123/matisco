using Prism.Regions;

namespace Matisco.Wpf.Prism
{
    public interface INeedsRegionManager
    {
        IRegionManager RegionManager { get; set; }
    }
}
