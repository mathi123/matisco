using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.Services;
using Matisco.Wpf;
using Matisco.Wpf.Interfaces;
//using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class UserOverviewModel : BindableBase, INavigationAware//, INeedsRegionManager
    {
        private readonly IWindowService _windosService;
        private readonly IUserService _userService;
        private readonly IRegionManager _regionManager;
        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value; 
                OnPropertyChanged();
            }
        }

        public User SelectedUser { get; set; }

        public ICommand OpenCommand => new DelegateCommand(Open);

        public UserOverviewModel(IWindowService windosService, IUserService userService, IRegionManager regionManager)
        {
            _windosService = windosService;
            _userService = userService;
            _regionManager = regionManager;
        }

        private void Open()
        {
            if (SelectedUser != null)
            {
                var navigationParameter = new NavigationParameters
                {
                    { "Id", SelectedUser.Id }
                };

                _regionManager.RequestNavigate(RegionNames.MainRegion, "UserView", navigationParameter);
                //_windosService.Open("UserView", SelectedUser.Id, navigationParameter, UserWindowClosed);
            }
        }

        private void UserWindowClosed(IResultDataCollection obj)
        {
            Task.Run(LoadUsersAsync);
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Task.Factory.StartNew(LoadUsersAsync);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private async Task LoadUsersAsync()
        {
            await Task.Run(() => Thread.Sleep(1000));

            Users = new ObservableCollection<User>(_userService.GetAll().ToList());
        }

        //public IRegionManager RegionManager { get; set; }
    }
}
