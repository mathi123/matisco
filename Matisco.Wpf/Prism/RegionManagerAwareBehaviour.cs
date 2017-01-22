using System;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;

namespace Matisco.Wpf.Prism
{
    public class RegionManagerAwareBehaviour : RegionBehavior
    {
        public const string BehaviourKey = "RegionManagerAwareBehaviour";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViewsChanged;
        }

        private void ActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IRegionManager regionManager = Region.RegionManager;
                    FrameworkElement element = item as FrameworkElement;

                    if (element != null)
                    {
                        IRegionManager scopedRegionManager =
                            element.GetValue(RegionManager.RegionManagerProperty) as IRegionManager;
                        
                        if (scopedRegionManager != null)
                        {
                            regionManager = scopedRegionManager;
                        }

                        InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }

        }

        static void InvokeOnRegionManagerAwareElement(object item, Action<INeedsRegionManager> invocation)
        {
            var rmAwareItem = item as INeedsRegionManager;
            if (rmAwareItem != null)
                invocation(rmAwareItem);

            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                var datacontext = frameworkElement.DataContext as INeedsRegionManager;
                if (datacontext != null)
                {
                    var parent = frameworkElement.Parent as FrameworkElement;
                    
                    if (parent != null)
                    {
                        var rmAwareDataContextParent = parent.DataContext as INeedsRegionManager;

                        if (rmAwareDataContextParent != null)
                        {
                            if (datacontext == rmAwareDataContextParent)
                            {
                                return;
                            }
                        }
                    }

                    invocation(datacontext);
                }
            }
        }
    }
}
