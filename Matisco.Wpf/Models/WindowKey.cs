using System;

namespace Matisco.Wpf.Models
{
    public class WindowKey
    {
        public string Type { get; }

        public object Key { get; }

        public WindowKey(Type windowType, object key)
        {
            Type = windowType.FullName;
            Key = key;
        }

        public WindowKey(string viewType, object key)
        {
            Type = viewType;
            Key = key;
        }

        public bool Equals(WindowKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Type == other.Type && Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != this.GetType()) return false;

            return Equals((WindowKey)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
