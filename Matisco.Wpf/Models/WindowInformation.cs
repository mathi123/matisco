using System;
using System.Windows;

namespace Matisco.Wpf.Models
{
    public class WindowInformation
    {
        public WindowKey Key { get; set; }

        public WindowKey ParentKey { get; set; }

        public WindowKey DialogChildKey { get; set; }

        public Window Window { get; set; }

        public Action<object[]> AfterWindowClosedAction { get; set;  }
        
        public WindowInformation()
        {
            
        }
    }
}
