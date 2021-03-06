using System.Collections.ObjectModel;
using System.Windows.Input;
using Example.BusinessApp.Infrastructure;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Sales.Services;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.Sales.ViewModels
{
    public class CustomerOverviewModel : BindableBase, INavigationAware, IHasTitle
    {
        private readonly ICustomerService _customerService;
        private readonly IWindowService _windowService;
        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value; 
                RaisePropertyChanged();
            }
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand OpenCommand => new DelegateCommand(OpenCustomer);

        public CustomerOverviewModel(ICustomerService customerService, IWindowService windowService)
        {
            _customerService = customerService;
            _windowService = windowService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ReloadList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        
        private void OpenCustomer()
        {
            if (SelectedCustomer != null)
            {
                var parameters = new NavigationParameters {{"id", SelectedCustomer.Id}};
                _windowService.OpenDialog(this, ViewNames.CustomerView, parameters, ReloadData);
            }
        }

        private void ReloadData(IResultDataCollection obj)
        {
            ReloadList();
        }

        private void ReloadList()
        {
            var customers = _customerService.GetAll();
            Customers = new ObservableCollection<Customer>(customers);
        }

        private void NavigationCallback(NavigationResult obj)
        {
            
        }

        public string GetTitle()
        {
            return "Customers";
        }
    }
}
