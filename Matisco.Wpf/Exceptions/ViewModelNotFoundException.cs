using System;

namespace Matisco.Wpf.Exceptions
{
    public class ViewModelNotFoundException : Exception
    {
        public ViewModelNotFoundException(Type viewType)
            : base("Viewmodel could not be located for the type " + viewType.FullName)
        {
        }
    }
}
