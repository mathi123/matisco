using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.Services;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class UserViewModel : BindableBase, IEditor, INavigationAware
    {
        private readonly IExceptionHandler _handler;
        private readonly IWindowService _windowService;
        private readonly IUserService _userService;
        private string _email;
        private string _name;
        private bool _isCool;
        private bool? _isCoolNullable;
        private User _user;
        private ObservableCollection<Language> _languages = new ObservableCollection<Language>(new Language[]
        {
            new Language()
            {
                Code = "nl",
                Description = "Nederlands"
            },
            new Language()
            {
                Code = "de",
                Description = "Duits"
            },
            new Language()
            {
                Code = "fr",
                Description = "Frans"
            }
        });

        private Language _language = new Language()
        {
            Code = "nl",
            Description = "Nederlands"
        };

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value; 
                OnPropertyChanged();
                Validate();
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

        public bool IsCool
        {
            get { return _isCool; }
            set
            {
                _isCool = value; 
                OnPropertyChanged();
            }
        }

        public bool? IsCoolNullable
        {
            get { return _isCoolNullable; }
            set
            {
                _isCoolNullable = value; 
                OnPropertyChanged();
            }
        }

        public Language Language
        {
            get { return _language; }
            set
            {
                _language = value; 
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand => new DelegateCommand(OkClicked);

        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value; 
                OnPropertyChanged();
            }
        }

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
                _user = _userService.GetById(id);

                Email = _user.Email;
                Name = _user.Name;
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

        private void Validate()
        {
            
        }

        public bool HasUnsavedChanges()
        {
            return _user?.Email != Email || _user?.Name != Name;
        }
    }
}
