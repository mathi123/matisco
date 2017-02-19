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
        private readonly IModalsService _modalsService;
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

        public CustomerViewModel(ICustomerService customerService, IWindowService windowService, IExceptionHandler exceptionHandler, IModalsService modalsService)
        {
            _customerService = customerService;
            _windowService = windowService;
            _exceptionHandler = exceptionHandler;
            _modalsService = modalsService;
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
        
        private void Save()
        {
            _modalsService.YesNoConfirm(this, (val) =>
            {
                _customerService.Save(_current);
                _original = _current.Clone();
            }, "Confirm", "Zeker dat u wilt opslaan?");
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
