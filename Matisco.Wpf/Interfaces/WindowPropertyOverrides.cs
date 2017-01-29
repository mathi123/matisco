using System.Windows;

namespace Matisco.Wpf.Interfaces
{
    public class WindowPropertyOverrides
    {
        public SizeToContent? SizeToContent { get; set; }

        public string Title { get; set; }

        public ResizeMode? ResizeMode { get; set; }

        public string IconPath { get; set; }
    }
}