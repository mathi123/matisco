using System.Windows.Input;
using Matisco.Wpf;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.SomeApplication.CustomerManagement
{
    public class CustomerViewModel : BindableBase, IEditor, INavigationAware
    {
        private readonly ICustomerService _customerService;
        private readonly IWindowService _windowService;
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

        public CustomerViewModel(ICustomerService customerService, IWindowService windowService)
        {
            _customerService = customerService;
            _windowService = windowService;
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
            _customerService.Save(_current);
            _original = _current.Clone();
        }

        private void OpenDialog()
        {
            _windowService.OpenDialog(this, nameof(ConfirmView), null, ConfirmBoxClosed);
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
    }
}
