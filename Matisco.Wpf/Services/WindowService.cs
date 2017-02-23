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
        private readonly WindowCollectionManager _windowCollection = new WindowCollectionManager();

        public WindowService(IContainer container, IEventAggregator eventAggregator, IShellInformationService shellInformationService,
            IRegionManager regionManager)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _shellInformationService = shellInformationService;
            _regionManager = regionManager;
        }

        public void OpenNewWindow<T>(NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null) where T : Window
        {
            OpenOrFocusWindow<T>(Guid.NewGuid(), parameters, onWindowClosedAction);
        }

        public void OpenOrFocusWindow<T>(object key, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), key);
            var windowToFocus = _windowCollection.Get(windowKey);

            if (!ReferenceEquals(windowToFocus, null))
            {
                windowToFocus.Window.Activate();
                return;
            }

            var window = CreateWindow(typeof(T), windowKey, onWindowClosedAction);

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
            var windowToFocus = _windowCollection.Get(key);

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

        public void OpenNewWindowAsDialog<T>(object parent, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), Guid.NewGuid());
            var parentWindowKey = FindWindowKeyContainingViewOrViewModel(parent);
            var parentWindow = _windowCollection.Get(parentWindowKey);

            if (ReferenceEquals(parentWindow, null)) return;

            var window = CreateWindow(typeof(T), windowKey, onWindowClosedAction);
            
            NavigateInWindow(window, parameters);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindow.Window;

            var information = new WindowInformation()
            {
                Key = windowKey,
                ParentKey = parentWindowKey,
                Window = window,
                AfterWindowClosedAction = onWindowClosedAction
            };

            _windowCollection.Add(information);

            window.Show();
        }

        public void Open(string viewType, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null)
        {
            Open(viewType, Guid.NewGuid(), parameters, onWindowClosedAction);
        }

        public void Open(string viewType, object key, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null)
        {
            var shellType = _shellInformationService.GetShellType();
            var windowKey = new WindowKey(viewType, key);

            var existingWindow = _windowCollection.Get(windowKey);
            if (!ReferenceEquals(existingWindow, null))
            {
                existingWindow.Window.Activate();
                return;
            }

            var window = CreateWindow(shellType, windowKey, onWindowClosedAction);
            var shellWindow = window as DependencyObject;

            var regionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shellWindow, regionManager);
            RegionManagerAware.SetRegionManagerAware(shellWindow, regionManager);

            regionManager.RequestNavigate(RegionNames.MainRegion, viewType, parameters);

            window.Show();
        }

        public void OpenDialog(object parent, string viewType, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null)
        {
            var parentWindowKey = FindWindowKeyContainingViewOrViewModel(parent);
            var parentWindowInfo = _windowCollection.Get(parentWindowKey);

            if (ReferenceEquals(parentWindowInfo,null)) return;

            var shellType = _shellInformationService.GetShellType();
            var windowKey = new WindowKey(viewType, Guid.NewGuid());
            var window = CreateWindow(shellType, windowKey, onWindowClosedAction);

            // Set region manager
            var shellWindow = window as DependencyObject;
            var regionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shellWindow, regionManager);
            RegionManagerAware.SetRegionManagerAware(shellWindow, regionManager);
            regionManager.RequestNavigate(RegionNames.MainRegion, viewType, parameters);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindowInfo.Window;

            var windowInformation = new WindowInformation()
            {
                Key = windowKey,
                ParentKey = parentWindowKey,
                Window = window,
                AfterWindowClosedAction = onWindowClosedAction
            };

            _windowCollection.Add(windowInformation);

            parentWindowInfo.Window.IsEnabled = false;

            window.Show();
        }

        public void CloseWindow(WindowKey key)
        {
            var windowInformation = _windowCollection.Get(key);

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
            foreach (var windowInformation in _windowCollection.GetWindowInformations())
            {
                yield return windowInformation.Window;
            }
        }

        public IEnumerable<WindowKey> GetWindowKeys()
        {
            foreach (var windowInformation in _windowCollection.GetWindowInformations())
            {
                yield return windowInformation.Key;
            }
        }

        public IEnumerable<WindowInformation> GetWindowInformations()
        {
            return _windowCollection.GetWindowInformations();
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

        private Window CreateWindow(Type type, WindowKey windowKey, Action<object[]> onWindowClosedAction)
        {
            var window = _container.Resolve(type) as Window;

            var windowInformation = new WindowInformation()
            {
                Key = windowKey,
                AfterWindowClosedAction = onWindowClosedAction,
                Window = window
            };

            _windowCollection.Add(windowInformation);
            
            window.Closed += WindowClosedCallback;
            window.Activated += WindowActivateCallback;

            return window;
        }

        private void WindowActivateCallback(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (ReferenceEquals(window, null))
                return;

            var windowInformation = _windowCollection.SearchByWindow(window);
            if (ReferenceEquals(windowInformation, null))
                return;

            if(ReferenceEquals(windowInformation.DialogChildKey, null))
                return;

            var dialog = _windowCollection.Get(windowInformation.DialogChildKey);
            dialog?.Window.Activate();
        }

        private void WindowClosedCallback(object sender, EventArgs eventArgs)
        {
            var window = _windowCollection.SearchByWindow(sender as Window);

            if (window.AfterWindowClosedAction != null)
            {
                var results = ExtractResultsFromViewModel(window.Window);
                window.AfterWindowClosedAction(results);
            }

            _windowCollection.Remove(window);

            CheckIfNoWindows();
        }

        private object[] ExtractResultsFromViewModel(Window window)
        {
            if (window is IHasResults) return (window as IHasResults).GetResults();

            var dataContext = window.DataContext as IHasResults;
            if (dataContext != null) return dataContext.GetResults();
             
            var manager = RegionManager.GetRegionManager(window);

            if (manager != null)
            {
                foreach (var region in manager.Regions)
                {
                    foreach (var view in region.ActiveViews)
                    {
                        if (view is IHasResults) return (view as IHasResults).GetResults();

                        var viewDataContext = (view as FrameworkElement)?.DataContext as IHasResults;
                        if (viewDataContext != null) return viewDataContext.GetResults();
                    }
                }
            }

            return null;
        }

        private void CheckIfNoWindows()
        {
            if (_windowCollection.IsEmpty())
            {
                _eventAggregator.GetEvent<AllWindowsClosedEvent>().Publish();
            }
        }

        private WindowKey FindWindowKeyContainingViewOrViewModel(object viewOrViewmodel)
        {
            if (ReferenceEquals(viewOrViewmodel, null)) return null;

            foreach (var keyValue in _windowCollection.GetWindowInformations())
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
