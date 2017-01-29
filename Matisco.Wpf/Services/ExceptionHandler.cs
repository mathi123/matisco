using System;
using Matisco.Wpf.ViewModels;
using Matisco.Wpf.Views;
using Prism.Regions;

namespace Matisco.Wpf.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IWindowService _windowService;

        public ExceptionHandler(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Handle(object viewOrViewModel, Exception exception)
        {
            Handle(viewOrViewModel, exception, null);
        }

        public void Handle(object viewOrViewModel, Exception exception, string messageToUser)
        {
            if (!string.IsNullOrEmpty(messageToUser))
            {
                exception = new Exception(messageToUser, exception);
            }

            var parameters = new NavigationParameters { { ExceptionViewModel.NavigationParameterException, exception } };

            if (viewOrViewModel == null)
            {
                _windowService.Open(nameof(ExceptionView), parameters);
            }
            else
            {
                _windowService.OpenDialog(viewOrViewModel, nameof(ExceptionView), parameters);
            }
        }
    }
}
