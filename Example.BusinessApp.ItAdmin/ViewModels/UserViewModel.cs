using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Example.BusinessApp.Infrastructure.Services;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class UserViewModel : BindableBase, INavigationAware
    {
        private readonly IExceptionHandler _handler;
        private readonly IWindowService _windowService;
        private readonly IUserService _userService;
        private string _email;
        private string _name;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value; 
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand => new DelegateCommand(OkClicked);

        public UserViewModel(IExceptionHandler handler, IWindowService windowService, IUserService userService)
        {
            _handler = handler;
            _windowService = windowService;
            _userService = userService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = (int) navigationContext.Parameters["Id"];

            Task.Factory.StartNew(() => LoadUserAsync(id));
        }

        private async Task LoadUserAsync(int id)
        {
            try
            {
                await Task.Run(() => Thread.Sleep(1000));
                var user = _userService.GetById(id);

                Email = user.Email;
                Name = user.Name;
            }
            catch (Exception ex)
            {
                _handler.Handle(this, ex);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return navigationContext.Parameters["Id"] is int;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OkClicked()
        {
            _windowService.CloseContainingWindow(this);
        }

    }
}
