using System;
using System.Windows.Input;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.ViewModels;
using Example.BusinessApp.Sales.Services;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.Sales.ViewModels
{
    public class CustomerViewModel : BindableBase, IEditor, INavigationAware, IHasTitle
    {
        private readonly ICustomerService _customerService;
        private readonly IWindowService _windowService;
        private readonly IExceptionHandler _exceptionHandler;
        private Customer _original;
        private Customer _current = new Customer();

        public string Name
        {
            get { return _current?.Name; }
            set
            {
                _current.Name = value; 
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _current?.Email; }
            set
            {
                _current.Email = value; 
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand => new DelegateCommand(Save);
        
        public ICommand OpenDialogCommand => new DelegateCommand(OpenDialog);

        public ICommand ThrowExceptionCommand => new DelegateCommand(ThrowException);

        public ICommand ThrowSmallExceptionCommand => new DelegateCommand(ThrowSmallException);
        
        public CustomerViewModel(ICustomerService customerService, IWindowService windowService, IExceptionHandler exceptionHandler)
        {
            _customerService = customerService;
            _windowService = windowService;
            _exceptionHandler = exceptionHandler;
        }

        public bool HasUnsavedChanges()
        {
            return !_current.Equals(_original);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = (int) navigationContext.Parameters["id"];
            _original = _customerService.GetById(id);
            _current = _original.Clone();
            OnPropertyChanged("Name");
            OnPropertyChanged("Email");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var id = navigationContext.Parameters["id"];
            return id != null;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void ThrowException()
        {
            var exception = new ArgumentException("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel dignissim nunc. Donec iaculis enim tellus, at pretium mauris ultrices eu. Sed eros est, euismod et viverra eget, pellentesque nec diam. Vestibulum elementum tellus in lacus pellentesque, quis ullamcorper metus eleifend. Nullam id vestibulum elit. Nunc lacinia eleifend accumsan. Phasellus in neque purus. Sed hendrerit sit amet lacus eget luctus. Sed eget placerat augue. Duis aliquet purus purus, vitae vestibulum ligula euismod nec.");
            _exceptionHandler.Handle(this, exception, "Some error ocurred");
        }

        private void ThrowSmallException()
        {
            var exception = new ArgumentException("Missing projectId;");

            _exceptionHandler.Handle(this, exception);
        }

        private void Save()
        {
            _customerService.Save(_current);
            _original = _current.Clone();
        }

        private void OpenDialog()
        {
            _windowService.OpenDialog(this, nameof(Infrastructure.Views.ConfirmView), null, ConfirmBoxClosed);
        }

        private void ConfirmBoxClosed(object[] obj)
        {
            if (obj != null && obj.Length == 1 && obj[0] is ConfirmResult)
            {
                var result = (ConfirmResult)obj[0];

                if (result == ConfirmResult.Yes)
                {
                    _windowService.CloseContainingWindow(this);
                }
            }
        }

        public string GetTitle()
        {
            return "Edit customer";
        }
    }
}
