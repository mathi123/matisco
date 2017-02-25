using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

namespace Matisco.Wpf.Models
{
    public class ConcurrentWindowCollection
    {
        private readonly ConcurrentDictionary<WindowKey, WindowInformation> _windows = new ConcurrentDictionary<WindowKey, WindowInformation>();

        public void Add(WindowInformation information)
        {
            if (ReferenceEquals(information, null))
                return;

            _windows.TryAdd(information.Key, information);
            
            if (information.ParentKey != null)
            {
                var parent = Get(information.ParentKey);
                parent.DialogChildKey = information.Key;
            }
        }

        public WindowInformation Get(WindowKey key)
        {
            WindowInformation window;

            _windows.TryGetValue(key, out window);

            return window;
        }

        public WindowInformation GetParent(WindowKey key)
        {
            if (ReferenceEquals(key, null))
                return null;

            var information = Get(key);

            var parentKey = information?.ParentKey;

            if (ReferenceEquals(parentKey, null))
                return null;

            return Get(parentKey);
        }

        public bool ContainsKey(WindowKey key)
        {
            if (ReferenceEquals(key, null))
                return false;

            return _windows.ContainsKey(key);
        }

        public bool IsEmpty()
        {
            return _windows.IsEmpty;
        }

        public IEnumerable<WindowInformation> GetWindowInformations()
        {
            foreach (var keyValue in _windows)
            {
                yield return keyValue.Value;
            }
        }

        public WindowInformation SearchByWindow(Window window)
        {
            if (ReferenceEquals(window, null))
                return null;

            foreach (var information in GetWindowInformations())
            {
                if(ReferenceEquals(window, information.Window))
                    return information;
            }

            return null;
        }

        public void Remove(WindowInformation window)
        {
            WindowInformation info;
            _windows.TryRemove(window.Key, out info);

            if (!ReferenceEquals(info, null))
            {
                if (info.ParentKey != null)
                {
                    var parent = Get(info.ParentKey);

                    if (parent != null && ReferenceEquals(parent.DialogChildKey, info.Key))
                    {
                        parent.DialogChildKey = null;
                    }
                }
            }
        }
    }
}
