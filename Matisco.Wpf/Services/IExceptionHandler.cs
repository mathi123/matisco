using System;

namespace Matisco.Wpf.Services
{
    public interface IExceptionHandler
    {
        void Handle(object viewOrViewModel, Exception exception);

        void Handle(object viewOrViewModel, Exception exception, string messageToUser);
    }
}
