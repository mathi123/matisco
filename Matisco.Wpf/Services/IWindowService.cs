using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;
using Prism.Regions;

namespace Matisco.Wpf.Services
{
    public interface IWindowService
    {
        /// <summary>
        /// Create and show a new window instance.
        /// </summary>
        /// <typeparam name="T">Type of the view, this type should derive from Window.</typeparam>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="onWindowClosedAction">Callback action when window closes</param>
        void OpenNewWindow<T>(NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window;

        /// <summary>
        /// Create a new window instance, or focus it if it already exists.
        /// </summary>
        /// <typeparam name="T">Type of the view, this type should derive from Window.</typeparam>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="key">Key to determine if the window already exists.</param>
        /// <param name="onWindowClosedAction">Callback action when window closes</param>
        void OpenOrFocusWindow<T>(object key, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window;

        /// <summary>
        /// Focus an existing window.
        /// </summary>
        /// <typeparam name="T">Type of the view, this type should derive from Window.</typeparam>
        /// <param name="key">Key of the window.</param>
        void TryFocusWindow<T>(object key) where T : Window;

        /// <summary>
        /// Focus an existing window by its window key.
        /// </summary>
        /// <param name="key"></param>
        void TryFocusWindowByWindowKey(WindowKey key);

        /// <summary>
        /// Open a new window, as a dialog over the current window.
        /// </summary>
        /// <typeparam name="T">Type of the view, this type should derive from Window.</typeparam>
        /// <param name="parent">The parent view instance. The window will be opened on top of this one.</param>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="onWindowClosedAction">Action to execute after the window or view closes.</param>
        void OpenNewWindowAsDialog<T>(object parent, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null) where T : Window;

        /// <summary>
        /// Creates a new shell window, with a view inside.
        /// </summary>
        /// <typeparam name="T">Type of the view.</typeparam>
        /// <param name="viewType">Type of the view to load in shell.</param>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="onWindowClosedAction">Action to execute after the window or view closes.</param>
        void Open(string viewType, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null);

        /// <summary>
        /// Creates a new shell window, or focusses it when one already exists.
        /// </summary>
        /// <typeparam name="T">Type of the view.</typeparam>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="viewType">Type of the view to load in the shell.</param>
        /// <param name="key">Key to determine if the window already exists.</param>
        /// <param name="onWindowClosedAction">Callback action when window closes</param>
        void Open(string viewType, object key, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null);

        /// <summary>
        /// Opens a view in a fixed dialog.
        /// </summary>
        /// <param name="parent">The parent control or its viewmodel. The window will be opened on top of this one.</param>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="parameters">Prism navigation parameters.</param>
        /// <param name="onWindowClosedAction">Action to execute after the window or view closes.</param>
        void OpenDialog(object parent, string viewType, NavigationParameters parameters = null, Action<IResultDataCollection> onWindowClosedAction = null);

        /// <summary>
        /// Close a window
        /// </summary>
        /// <param name="key"></param>
        void CloseWindow(WindowKey key);

        /// <summary>
        /// Close the window containing the current view or viewmodel.
        /// </summary>
        /// <param name="viewOrViewModel"></param>
        void CloseContainingWindow(object viewOrViewModel);

        /// <summary>
        /// Get all the open windows.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Window> GetWindows();

        IRegionManager GetCurrentRegionManager();

        IEnumerable<WindowKey> GetWindowKeys();
    }
}
