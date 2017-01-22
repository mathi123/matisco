using System;

namespace Matisco.Wpf.Models
{
    public class WindowClosedAction
    {
        public WindowKey WindowKey { get; }

        public Action<object[]> AfterWindowClosedAction { get; }

        public WindowClosedAction(WindowKey windowKey, Action<object[]> afterWindowClosedAction)
        {
            WindowKey = windowKey;
            AfterWindowClosedAction = afterWindowClosedAction;
        }
    }
}
