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
using System.Diagnostics;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class UserViewModel : BindableBase, IEditor, INavigationAware, INotifyDataErrorInfo
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

        private bool _editModus;
        private string _editSaveMessage;
        private string _cancelCloseMessage;
        private int _id;
        private double _length;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
                Validate();
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

        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                Debug.WriteLine("written back:" + _length);
                OnPropertyChanged();
            }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set
            {
                _birthYear = value; 
                OnPropertyChanged();
            }
        }

        public bool EditModus
        {
            get { return _editModus; }
            set
            {
                _editModus = value;
                OnPropertyChanged();
            }
        }

        public string EditSaveMessage
        {
            get { return _editSaveMessage; }
            set
            {
                _editSaveMessage = value;
                OnPropertyChanged();
            }
        }

        public string CancelCloseMessage
        {
            get { return _cancelCloseMessage; }
            set
            {
                _cancelCloseMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand => new DelegateCommand(OkClicked);

        public ICommand CancelCommand => new DelegateCommand(Cancel);

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
            UpdateButtons();

            _id = (int)navigationContext.Parameters["Id"];

            Task.Factory.StartNew(LoadUserAsync);
        }

        private async Task LoadUserAsync()
        {
            try
            {
                await Task.Run(() => Thread.Sleep(1000));
                _user = _userService.GetById(_id);

                Email = _user.Email;
                Name = _user.Name;
                Length = 1.84;
                Language = Languages.FirstOrDefault();
                BirthYear = 1990;

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
            if (!EditModus)
            {
                ToggleEditMode();
            }
            else
            {
                Save();
            }
        }

        private void Save()
        {
            // Save data

            // Go to read only mode
            ToggleEditMode();
        }

        private void Cancel()
        {
            if (EditModus)
            {
                CancelEdit();
            }
            else
            {
                _windowService.CloseContainingWindow(this);
            }
        }

        private async void CancelEdit()
        {
            await LoadUserAsync();

            ToggleEditMode();
        }

        private void Validate()
        {
            RealValidation();
        }

        private void ToggleEditMode()
        {
            EditModus = !EditModus;

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (EditModus)
            {
                EditSaveMessage = "Save";
                CancelCloseMessage = "Cancel";
            }
            else
            {
                EditSaveMessage = "Edit";
                CancelCloseMessage = "Close";
            }
        }

        public bool HasUnsavedChanges()
        {
            return _user?.Email != Email || _user?.Name != Name;
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return _errors.Values.FirstOrDefault(l => l.Count > 0) != null; }
        }

        private Dictionary<string, List<string>> _errors =
            new Dictionary<string, List<string>>();
        private object _lock = new object();
        private int _birthYear;

        private void RealValidation()
        {
            lock (_lock)
            {
                //Validate Name
                List<string> errorsForName;
                if (!_errors.TryGetValue("Name", out errorsForName))
                    errorsForName = new List<string>();
                else errorsForName.Clear();

                if (String.IsNullOrEmpty(Name))
                    errorsForName.Add("The name can't be null or empty.");

                _errors["Name"] = errorsForName;
                if (errorsForName.Count > 0) RaiseErrorsChanged("Name");

                //Validate Email
                List<string> errorsForAge;
                if (!_errors.TryGetValue("Age", out errorsForAge))
                    errorsForAge = new List<string>();
                else errorsForAge.Clear();

                if (string.IsNullOrEmpty(Email))
                    errorsForAge.Add("The name can't be null or empty.");

                _errors["Email"] = errorsForAge;
                if (errorsForAge.Count > 0) RaiseErrorsChanged("Email");
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler == null) return;
            var arg = new DataErrorsChangedEventArgs(propertyName);
            handler.Invoke(this, arg);
        }
    }
}
