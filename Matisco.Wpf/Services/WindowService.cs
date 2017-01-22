using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        private readonly ConcurrentDictionary<WindowKey, Window> _openWindows = new ConcurrentDictionary<WindowKey, Window>();
        private readonly List<WindowClosedAction> _windowClosedActions = new List<WindowClosedAction>();
        private readonly ConcurrentDictionary<WindowKey, WindowKey> _openDialogs = new ConcurrentDictionary<WindowKey, WindowKey>();

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

            if (_openWindows.ContainsKey(windowKey))
            {
                Window windowToFocus;
                var windowWasFound = _openWindows.TryGetValue(windowKey, out windowToFocus);

                if (windowWasFound)
                {
                    // Todo: call on UI thread?
                    windowToFocus.Activate();
                    return;
                }
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
            Window windowToFocus;
            var windowWasFound = _openWindows.TryGetValue(key, out windowToFocus);

            if (windowWasFound)
            {
                // Todo: call on UI thread
                if (!windowToFocus.IsVisible)
                {
                    windowToFocus.Show();
                }
                else
                {
                    windowToFocus.Activate();
                }
            }
        }

        public void OpenNewWindowAsDialog<T>(object parent, NavigationParameters parameters = null, Action<object[]> onWindowClosedAction = null) where T : Window
        {
            var windowKey = new WindowKey(typeof(T), Guid.NewGuid());
            var parentWindowKey = FindWindowKeyContainingViewOrViewModel(parent);
            Window parentWindow;
            _openWindows.TryGetValue(parentWindowKey, out parentWindow);

            if (parentWindow == null) return;

            var window = CreateWindow(typeof(T), windowKey, onWindowClosedAction);

            NavigateInWindow(window, parameters);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindow;

            _openDialogs.TryAdd(parentWindowKey, windowKey);

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

            if (_openWindows.ContainsKey(windowKey))
            {
                Window windowToFocus;
                var windowWasFound = _openWindows.TryGetValue(windowKey, out windowToFocus);

                if (windowWasFound)
                {
                    // Todo: call on UI thread?
                    windowToFocus.Activate();
                    return;
                }
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
            Window parentWindow;
            _openWindows.TryGetValue(parentWindowKey, out parentWindow);

            if (parentWindow == null) return;

            var shellType = _shellInformationService.GetShellType();
            var windowKey = new WindowKey(viewType, Guid.NewGuid());
            var window = CreateWindow(shellType, windowKey, onWindowClosedAction);

            var shellWindow = window as DependencyObject;

            var regionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shellWindow, regionManager);
            RegionManagerAware.SetRegionManagerAware(shellWindow, regionManager);

            regionManager.RequestNavigate(RegionNames.MainRegion, viewType, parameters);

            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = parentWindow;
            
            _openDialogs.TryAdd(parentWindowKey, windowKey);

            window.Show();
        }

        public void CloseWindow(WindowKey key)
        {
            Window window;
            var windowExists = _openWindows.TryGetValue(key, out window);

            if (windowExists)
            {
                window.Close();
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

        public ConcurrentDictionary<WindowKey, Window> GetAllOpenWindows()
        {
            return _openWindows;
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

            _openWindows.TryAdd(windowKey, window);

            window.Closed += WindowClosedCallback;
            window.Activated += WindowActivateCallback;

            if (onWindowClosedAction != null)
            {
                var windowClosedAction = new WindowClosedAction(windowKey, onWindowClosedAction);
                _windowClosedActions.Add(windowClosedAction);
            }

            return window;
        }

        private void WindowActivateCallback(object sender, EventArgs e)
        {
            var window = sender as Window;

            if (window != null)
            {
                var exists = _openWindows.Any(pair => Equals(pair.Value, window));

                if (exists)
                {
                    var activatedWindow = _openWindows.SingleOrDefault(pair => Equals(pair.Value, window));

                    WindowKey dialogKey;

                    var success = _openDialogs.TryGetValue(activatedWindow.Key, out dialogKey);

                    if (success && dialogKey != null)
                    {
                        Window dialogWindow;
                        _openWindows.TryGetValue(dialogKey, out dialogWindow);

                        dialogWindow?.Activate();
                    }
                }
            }
        }

        private void WindowClosedCallback(object sender, EventArgs eventArgs)
        {
            var window = sender as Window;

            if (window != null)
            {
                var exists = _openWindows.Any(pair => Equals(pair.Value, window));

                if (exists)
                {
                    var keyValuePair = _openWindows.SingleOrDefault(pair => Equals(pair.Value, window));
                    Window removedWindow;

                    var removed = _openWindows.TryRemove(keyValuePair.Key, out removedWindow);

                    var results = ExtractResultsFromViewModel(window);
                    foreach (var action in _windowClosedActions.Where(w => w.WindowKey.Equals(keyValuePair.Key)).ToArray())
                    {
                        // Todo: care with threading here
                        action.AfterWindowClosedAction(results);
                        _windowClosedActions.Remove(action);
                    }

                    foreach (var dialog in _openDialogs.ToArray())
                    {
                        if (ReferenceEquals(dialog.Value, window))
                        {
                            WindowKey key;
                            _openDialogs.TryRemove(dialog.Key, out key);
                            break;
                        }
                    }

                    if (removed)
                    {
                        CheckIfNoWindows();
                    }
                }
            }
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
            if (_openWindows.Count == 0)
            {
                _eventAggregator.GetEvent<AllWindowsClosedEvent>().Publish();
            }
        }

        private WindowKey FindWindowKeyContainingViewOrViewModel(object viewOrViewmodel)
        {
            if (ReferenceEquals(viewOrViewmodel, null)) return null;

            foreach (var keyValue in _openWindows)
            {
                var window = keyValue.Value;

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
