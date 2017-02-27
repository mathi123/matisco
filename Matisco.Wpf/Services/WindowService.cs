using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Matisco.Wpf.Events;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Matisco.Wpf.Prism;
using Prism.Events;
using Prism.Regions;

namespace Matisco.Wpf.Services
{
    public class WindowService : IWindowService
    {
        private readonly IContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IShellInformationService _shellInformationService;
        private readonly IRegionManager _regionManager;
        private readonly ConcurrentWindowCollection _concurrentWindowCollection = new ConcurrentWindowCollection();

        public WindowService(IContainer container, IEventAggregator eventAggregator, IShellInformationService shellInformationService,
            IRegionManager regionManager)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _shellInformationService = shellInformationService;
            _regionManager = regionManager;
        }

        public void OpenNewWindow<T>(NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window
        {
            OpenOrFocusWindow<T>(Guid.NewGuid(), parameters, onWindowClosedAction);
        }

        public void OpenOrFocusWindow<T>(object key, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), key);
            var windowToFocus = _concurrentWindowCollection.Get(windowKey);

            if (!ReferenceEquals(windowToFocus, null))
            {
                // Window already exists
                windowToFocus.Window.Activate();
                return;
            }

            var window = CreateWindow(typeof(T), windowKey, onWindowClosedAction, null, null);

            NavigateInWindow(window, parameters);
            
            window.Show();
        }

        public void TryFocusWindow<T>(object key) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), key);
            TryFocusWindowByWindowKey(windowKey);
        }

        public void TryFocusWindowByWindowKey(WindowKey key)
        {
            var windowToFocus = _concurrentWindowCollection.Get(key);

            if (!ReferenceEquals(windowToFocus, null))
            {
                var window = windowToFocus.Window;

                if (!window.IsVisible)
                {
                    window.Show();
                }
                else
                {
                    window.Activate();
                }
            }
        }

        public void OpenNewWindowAsDialog<T>(object parent, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), Guid.NewGuid());
            var parentWindowKey = FindWindowKeyContainingViewOrViewModel(parent);
            var parentWindow = _concurrentWindowCollection.Get(parentWindowKey);

            if (ReferenceEquals(parentWindow, null)) return;

            var window = CreateWindow(typeof(T), windowKey, onWindowClosedAction, null, null);
            
            NavigateInWindow(window, parameters);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindow.Window;

            window.Show();
        }

        public void Open(string viewType, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null)
        {
            Open(viewType, Guid.NewGuid(), parameters, onWindowClosedAction);
        }

        public void Open(string viewType, object key, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null)
        {
            var shellType = _shellInformationService.GetShellType();
            var windowKey = new WindowKey(viewType, key);

            var existingWindow = _concurrentWindowCollection.Get(windowKey);
            if (!ReferenceEquals(existingWindow, null))
            {
                existingWindow.Window.Activate();
                return;
            }

            var regionManager = _regionManager.CreateRegionManager();
            var window = CreateWindow(shellType, windowKey, onWindowClosedAction, null, regionManager);
            var shellWindow = window as DependencyObject;

            RegionManager.SetRegionManager(shellWindow, regionManager);
            //RegionManagerAware.SetRegionManagerAware(shellWindow, regionManager);

            regionManager.RequestNavigate(RegionNames.MainRegion, viewType, parameters);

            window.Show();
        }

        public void OpenDialog(object parent, string viewType, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null)
        {
            var parentWindowKey = FindWindowKeyContainingViewOrViewModel(parent);
            var parentWindowInfo = _concurrentWindowCollection.Get(parentWindowKey);

            if (ReferenceEquals(parentWindowInfo,null)) return;

            var shellType = _shellInformationService.GetShellType();
            var regionManager = _regionManager.CreateRegionManager();
            var windowKey = new WindowKey(viewType, Guid.NewGuid());
            var window = CreateWindow(shellType, windowKey, onWindowClosedAction, parentWindowKey, regionManager);

            // Set region manager
            var shellWindow = window as DependencyObject;
            RegionManager.SetRegionManager(shellWindow, regionManager);
            //RegionManagerAware.SetRegionManagerAware(shellWindow, regionManager);
            regionManager.RequestNavigate(RegionNames.MainRegion, viewType, parameters);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindowInfo.Window;
            //window.ShowInTaskbar = false;

            BlockWindow(parentWindowKey);

            window.Show();
        }

        public void CloseWindow(WindowKey key)
        {
            var windowInformation = _concurrentWindowCollection.Get(key);

            if (!ReferenceEquals(windowInformation, null))
            {
                windowInformation.Window.Close();
            }
        }

        public void CloseContainingWindow(object viewOrViewModel)
        {
            var windowKey = FindWindowKeyContainingViewOrViewModel(viewOrViewModel);

            if (windowKey != null)
            {
                CloseWindow(windowKey);
            }
        }

        public IEnumerable<Window> GetWindows()
        {
            foreach (var windowInformation in _concurrentWindowCollection.GetWindowInformations())
            {
                yield return windowInformation.Window;
            }
        }

        public IEnumerable<WindowKey> GetWindowKeys()
        {
            foreach (var windowInformation in _concurrentWindowCollection.GetWindowInformations())
            {
                yield return windowInformation.Key;
            }
        }

        public IEnumerable<WindowInformation> GetWindowInformations()
        {
            return _concurrentWindowCollection.GetWindowInformations();
        }

        private void NavigateInWindow(Window window, NavigationParameters parameters)
        {
            var context = new NavigationContext(null, null, parameters);

            var navigationWindow = window as INavigationAware;
            navigationWindow?.OnNavigatedTo(context);

            var frameworkElement = window as FrameworkElement;
            var dataContext = frameworkElement?.DataContext as INavigationAware;
            dataContext?.OnNavigatedTo(context);
        }

        private Window CreateWindow(Type type, WindowKey windowKey, Action<IResultDataCollection> onWindowClosedAction, WindowKey parentWindowKey, IRegionManager regionManager)
        {

            Window window;
            if (regionManager != null)
            {
                Debug.WriteLine($"view regionmanager: {regionManager.GetHashCode()}");
                Push(regionManager);

                window = _container.Resolve(type, new TypedParameter(typeof(IRegionManager), regionManager)) as Window;
            }
            else
            {
                window = _container.Resolve(type) as Window;
            }

            if (ReferenceEquals(window, null))
                return null;

            var windowInformation = new WindowInformation(windowKey, parentWindowKey, window, onWindowClosedAction);

            _concurrentWindowCollection.Add(windowInformation);
            
            window.Closed += WindowClosedCallback;
            window.Activated += WindowActivateCallback;
            window.Deactivated += WindowDeactivatedCallback;
            window.SizeChanged += WindowSizeChangedCallback;
            window.LocationChanged += WindowLocationChangedCallback;

            return window;
        }

        private IRegionManager _pendingRegionManager;

        private void Push(IRegionManager regionManager)
        {
            _pendingRegionManager = regionManager;
        }

        public IRegionManager GetCurrentRegionManager()
        {
            return _pendingRegionManager;
        }

        private void WindowLocationChangedCallback(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _concurrentWindowCollection.SearchByWindow(window);
            if (ReferenceEquals(windowInformation, null))
                return;

            if (ReferenceEquals(windowInformation.DialogChildKey, null))
                return;

            var dialog = _concurrentWindowCollection.Get(windowInformation.DialogChildKey);
            CenterOverParent(window, dialog.Window);
        }

        private void CenterOverParent(Window parentWindow, Window childWindow)
        {
            var baseX = parentWindow.Left + parentWindow.ActualWidth / 2;
            var baseY = parentWindow.Top + parentWindow.ActualHeight / 2;

            var left = childWindow.ActualWidth/2;
            var top = childWindow.ActualHeight/2;

            childWindow.Left = baseX - left;
            childWindow.Top = baseY - top;
        }

        private void WindowSizeChangedCallback(object sender, SizeChangedEventArgs e)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _concurrentWindowCollection.SearchByWindow(window);
            if (ReferenceEquals(windowInformation, null))
                return;

            if (ReferenceEquals(windowInformation.DialogChildKey, null))
                return;

            var dialog = _concurrentWindowCollection.Get(windowInformation.DialogChildKey);
            CenterOverParent(window, dialog.Window);
        }

        private void WindowActivateCallback(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _concurrentWindowCollection.SearchByWindow(window);
            if (ReferenceEquals(windowInformation, null))
                return;

            if(ReferenceEquals(windowInformation.DialogChildKey, null))
                return;

            var dialog = _concurrentWindowCollection.Get(windowInformation.DialogChildKey);
            dialog?.Window.Activate();
        }


        private void WindowDeactivatedCallback(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _concurrentWindowCollection.SearchByWindow(window);
            if (ReferenceEquals(windowInformation, null))
                return;

            if (windowInformation.CloseOnDeactivation)
            {
                CloseWindow(windowInformation.Key);
            }
        }

        private void WindowClosedCallback(object sender, EventArgs eventArgs)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _concurrentWindowCollection.SearchByWindow(window);

            if (windowInformation.AfterWindowClosedAction != null)
            {
                var results = ExtractResultsFromViewModel(windowInformation.Window);
                windowInformation.AfterWindowClosedAction(results);
            }

            if (!ReferenceEquals(windowInformation.ParentKey, null))
            {
                UnBlockWindow(windowInformation.ParentKey);
            }

            _concurrentWindowCollection.Remove(windowInformation);

            CheckIfNoWindows();
        }

        private void BlockWindow(WindowKey key)
        {
            var window = _concurrentWindowCollection.Get(key);

            if (ReferenceEquals(window, null))
                return;

            window.Window.Opacity = 0.5;
            window.Window.IsHitTestVisible = false;
        }

        private void UnBlockWindow(WindowKey key)
        {
            var window = _concurrentWindowCollection.Get(key);

            if (ReferenceEquals(window, null))
                return;

            window.Window.Opacity = 1;
            window.Window.IsHitTestVisible = true;
        }

        private IResultDataCollection ExtractResultsFromViewModel(Window window)
        {
            var data = new ResultDataCollection();

            // If the window implements IHasResults
            var windowResultProvider = window as IHasResults;
            if (!ReferenceEquals(windowResultProvider, null))
            {
                data.AddResult(window.GetType().FullName, windowResultProvider.GetResults());
            }

            // If the datacontext of the window implements IHasResults, return those results
            var datacontextResultProvider = window.DataContext as IHasResults;
            if (!ReferenceEquals(datacontextResultProvider, null))
            {
                data.AddResult(datacontextResultProvider.GetType().FullName, datacontextResultProvider.GetResults());
            }

            // Add the results from the views
            var manager = RegionManager.GetRegionManager(window);
            if (!ReferenceEquals(manager, null))
            {
                foreach (var region in manager.Regions)
                {
                    var view = region.ActiveViews.Single();

                    var viewResultsProvider = view as IHasResults;
                    if (!ReferenceEquals(viewResultsProvider, null))
                    {
                        data.AddResult(region.Name, viewResultsProvider.GetResults());
                    }

                    var viewDataContextResultsProvider = (view as FrameworkElement)?.DataContext as IHasResults;
                    if (!ReferenceEquals(viewDataContextResultsProvider, null))
                    {
                        data.AddResult(region.Name, viewDataContextResultsProvider.GetResults());
                    }
                }
            }

            return data;
        }

        private void CheckIfNoWindows()
        {
            if (_concurrentWindowCollection.IsEmpty())
            {
                _eventAggregator.GetEvent<AllWindowsClosedEvent>().Publish();
            }
        }

        private WindowKey FindWindowKeyContainingViewOrViewModel(object viewOrViewmodel)
        {
            if (ReferenceEquals(viewOrViewmodel, null)) return null;

            foreach (var keyValue in _concurrentWindowCollection.GetWindowInformations())
            {
                var window = keyValue.Window;

                if (ReferenceEquals(viewOrViewmodel, window)) return keyValue.Key;
                if (ReferenceEquals(viewOrViewmodel, window.DataContext)) return keyValue.Key;

                var manager = RegionManager.GetRegionManager(window);
                
                foreach (var region in manager.Regions)
                {
                    foreach (var view in region.ActiveViews)
                    {
                        if (ReferenceEquals(viewOrViewmodel, view)) return keyValue.Key;
                        if (ReferenceEquals(viewOrViewmodel, (view as FrameworkElement)?.DataContext)) return keyValue.Key;
                    }
                }
            }

            return null;
        }
    }
}
