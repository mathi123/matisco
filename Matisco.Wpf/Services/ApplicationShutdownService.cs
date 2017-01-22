using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Matisco.Wpf.Events;
using Matisco.Wpf.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Matisco.Wpf.Services
{
    public class ApplicationShutdownService : IApplicationShutdownService
    {
        private readonly IWindowService _windowService;
        private readonly List<string> _unsavedWindows = new List<string>();
        private bool _isTerminating;

        public ApplicationShutdownService(IEventAggregator eventAggregator, IWindowService windowService)
        {
            _windowService = windowService;

            eventAggregator.GetEvent<AllWindowsClosedEvent>().Subscribe(AfterAllWindowsClosed);
        }

        public List<string> GetBlockingWindows()
        {
            return _unsavedWindows;
        }

        public void ExitApplication(bool forceExit)
        {
            _isTerminating = true;
            _unsavedWindows.Clear();

            if (!forceExit && ApplicationHasUnsavedChanges())
            {
                Debug.WriteLine("Could not close application because there are unsaved changes.");
                _isTerminating = false;

                _windowService.Open(nameof(Views.UnsavedChangesView), 1);

                return;
            }

            var windows = _windowService.GetAllOpenWindows();
            foreach (var key in windows.Keys.ToArray())
            {
                _windowService.CloseWindow(key);
            }
        }

        private bool ApplicationHasUnsavedChanges()
        {
            var hasUnsavedChanges = false;
            var windows = _windowService.GetAllOpenWindows();

            foreach (var key in windows.Keys)
            {
                Window window;
                var hasWindow = windows.TryGetValue(key, out window);
                if (!hasWindow) continue;

                bool windowHasUnsavedChanges = false;

                var windowAsEditor = window as IEditor;
                if (windowAsEditor != null)
                {
                    windowHasUnsavedChanges = windowAsEditor.HasUnsavedChanges();
                }

                var frameworkElement = window as FrameworkElement;
                var dataContext = frameworkElement?.DataContext as IEditor;
                if (dataContext != null)
                {
                    windowHasUnsavedChanges = windowHasUnsavedChanges || dataContext.HasUnsavedChanges();
                }

                windowHasUnsavedChanges = windowHasUnsavedChanges || ChildRegionsHaveUnsavedChanges(window);

                if (windowHasUnsavedChanges)
                {
                    _unsavedWindows.Add(window.Title);
                }

                hasUnsavedChanges = hasUnsavedChanges || windowHasUnsavedChanges;
            }

            return hasUnsavedChanges;
        }

        private bool ChildRegionsHaveUnsavedChanges(Window window)
        {
            var manager = RegionManager.GetRegionManager(window);

            if (manager != null)
            {
                foreach (var region in manager.Regions)
                {
                    foreach (var view in region.ActiveViews)
                    {
                        var viewEditor = view as IEditor;

                        if (viewEditor != null)
                        {
                            if (viewEditor.HasUnsavedChanges())
                                return true;
                        }

                        var frameworkElement = view as FrameworkElement;
                        var dataContext = frameworkElement?.DataContext as IEditor;

                        if (dataContext != null)
                        {
                            if (dataContext.HasUnsavedChanges())
                                return true;
                        }
                    }
                }    
            }

            return false;
        }

        private void AfterAllWindowsClosed()
        {
            if (_isTerminating)
            {
                // all the windows where closed
                Application.Current.Shutdown();
            }
        }
    }
}
