using System.Windows.Input;
using Example.BusinessApp.Infrastructure.Services;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Example.BusinessApp.Infrastructure.Models;
using Example.BusinessApp.Infrastructure.Screens;
using Matisco.Wpf.Controls.Buttons;
using Matisco.Wpf.Interfaces;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class RoleViewModel :  EditScreenBase
    {
        public const string ParameterRoleId = "RoleId";

        private readonly IRoleService _roleService;
        private readonly IWindowService _windowService;
        private readonly IModalsService _modalsService;
        private Role _model;
        private Role _originalModel;

        public string Description
        {
            get { return _model?.Description; }
            set
            {
                _model.Description = value;
                OnPropertyChanged();
            }
        }

        public RoleViewModel(IRoleService roleService, IWindowService windowService, IModalsService modalsService)
        {
            _roleService = roleService;
            _windowService = windowService;
            _modalsService = modalsService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _originalModel = _roleService.GetById((int) navigationContext.Parameters[ParameterRoleId]);
            SetModel(_originalModel.Clone());
            SetEditMode(false);
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return navigationContext.Parameters[ParameterRoleId] is int;
        }

        private void SetModel(Role role)
        {
            _model = role;
            OnPropertyChanged(nameof(Description));
        }

        protected override void Close()
        {
            _windowService.CloseContainingWindow(this);
        }

        protected override void Cancel()
        {
            SetModel(_originalModel.Clone());
            SetEditMode(false);
        }

        protected override void Edit()
        {
            SetEditMode(true);
        }

        protected override void Save()
        {
            var result = _roleService.Update(_model);

            if (result.Succes)
            {
                _originalModel = _model.Clone();
                SetEditMode(false);
            }
            else
            {
                var errorMessage = "Het opslaan is mislukt.";

                foreach (var resultValidationError in result.ValidationErrors)
                {
                    errorMessage += "\n" + resultValidationError;
                }

                _modalsService.WarningMessage(this, "Mislukt", errorMessage);
            }
        }

        public override bool HasUnsavedChanges()
        {
            return _originalModel.Description != _model.Description;
        }
    }
}
