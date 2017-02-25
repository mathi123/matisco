using System;
using System.Windows;
using Matisco.Wpf.Interfaces;

namespace Matisco.Wpf.Models
{
    public class WindowInformation
    {
        public WindowKey Key { get; private set; }

        public WindowKey ParentKey { get; private set; }

        public Window Window { get; private set; }

        public Action<IResultDataCollection> AfterWindowClosedAction { get; private set; }

        public WindowKey DialogChildKey { get; set; }

        public WindowInformation(WindowKey key, WindowKey parentKey, Window window,
            Action<IResultDataCollection> afterWindowClosedAction)
        {
            Key = key;
            ParentKey = parentKey;
            Window = window;
            AfterWindowClosedAction = afterWindowClosedAction;
        }
    }
}
