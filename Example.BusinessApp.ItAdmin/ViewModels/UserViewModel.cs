using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.Services;
using Matisco.Wpf.Services;
using Prism.Regions;
using Example.BusinessApp.Infrastructure.Screens;
using Matisco.Core;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class UserViewModel : EditScreenBase
    {
        private readonly IExceptionHandler _handler;
        private readonly IWindowService _windowService;
        private readonly IUserService _userService;

        private User _model = new User();
        private User _originalModel;

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
            get { return _model.Email; }
            set
            {
                _model.Email = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCool
        {
            get { return _model.IsCool; }
            set
            {
                _model.IsCool = value;
                RaisePropertyChanged();
            }
        }

        public bool? IsCoolNullable
        {
            get { return _model.IsCoolNullable; }
            set
            {
                _model.IsCoolNullable = value;
                RaisePropertyChanged();
            }
        }

        public Language Language
        {
            get { return _language; }
            set
            {
                _language = value;
                RaisePropertyChanged();
            }
        }

        public double Length
        {
            get
            {
                return _model.Length;
            }
            set
            {
                _model.Length = value;
                RaisePropertyChanged();
            }
        }

        public int BirthYear
        {
            get { return _model.BirthYear; }
            set
            {
                _model.BirthYear = value; 
                RaisePropertyChanged();
            }
        }

        public decimal NetValue
        {
            get { return _model.NetValue; }
            set
            {
                _model.NetValue = value; 
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                RaisePropertyChanged();
            }
        }

        public UserViewModel(IExceptionHandler handler, IWindowService windowService, IUserService userService,
            ITranslationService translationService) : base(translationService)
        {
            _handler = handler;
            _windowService = windowService;
            _userService = userService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SetEditMode(false);
            var id = (int)navigationContext.Parameters["Id"];

            Task.Factory.StartNew(() => LoadUserAsync(id));
        }

        public async Task LoadUserAsync(int id)
        {
            try
            {
                await Task.Run(() => Thread.Sleep(1000));
                _originalModel = _userService.GetById(id);
                SetModel(_originalModel.Clone());
            }
            catch (Exception ex)
            {
                _handler.Handle(this, ex);
            }
        }

        private void SetModel(User user)
        {
            _model = user;

            RaisePropertyChanged(nameof(Email));
            RaisePropertyChanged(nameof(Language));
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(NetValue));
            RaisePropertyChanged(nameof(BirthYear));
            RaisePropertyChanged(nameof(IsCool));
            RaisePropertyChanged(nameof(IsCoolNullable));
            RaisePropertyChanged(nameof(Length));
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return navigationContext.Parameters["Id"] is int;
        }

        protected override void Cancel()
        {
            SetModel(_originalModel);
            SetEditMode(false);
        }

        protected override void Close()
        {
            _windowService.CloseContainingWindow(this);
        }

        protected override void Edit()
        {
            SetEditMode(true);
        }

        protected override void Save()
        {
            _originalModel = _model.Clone();
            SetEditMode(false);
        }
    }
}
