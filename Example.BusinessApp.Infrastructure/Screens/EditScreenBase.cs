using System.Windows.Input;
using Matisco.Wpf.Controls.Buttons;
using Matisco.Wpf.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Example.BusinessApp.Infrastructure.Screens
{
    public abstract class EditScreenBase : ScreenBase, INavigationAware, IEditor
    {
        private bool _editMode;
        private string _editSaveMessage;
        private string _cancelCloseMessage;
        private ButtonImageEnum _editSaveButtonImage;
        private ButtonImageEnum _cancelCloseButtonImage;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
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

        public ButtonImageEnum EditSaveButtonImage
        {
            get { return _editSaveButtonImage; }
            set
            {
                _editSaveButtonImage = value;
                OnPropertyChanged();
            }
        }

        public ButtonImageEnum CancelCloseButtonImage
        {
            get { return _cancelCloseButtonImage; }
            set
            {
                _cancelCloseButtonImage = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditSaveCommand => new DelegateCommand(EditOrSave);

        public ICommand CancelCloseCommand => new DelegateCommand(CancelOrClose);
        
        public abstract void OnNavigatedTo(NavigationContext navigationContext);

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        protected void SetEditMode(bool mode)
        {
            EditMode = mode;
            if (mode)
            {
                EditSaveMessage = Translate("Save");
                CancelCloseMessage = Translate("Cancel");
                EditSaveButtonImage = ButtonImageEnum.Check;
                CancelCloseButtonImage = ButtonImageEnum.WindowClose;
            }
            else
            {
                EditSaveMessage = Translate("Edit");
                CancelCloseMessage = Translate("Close");
                EditSaveButtonImage = ButtonImageEnum.Edit;
                CancelCloseButtonImage = ButtonImageEnum.Clear;
            }
        }

        protected abstract void Cancel();

        protected abstract void Close();

        protected abstract void Edit();

        protected abstract void Save();

        private void CancelOrClose()
        {
            if (EditMode)
            {
                Cancel();
            }
            else
            {
                Close();
            }
        }

        private void EditOrSave()
        {
            if (EditMode)
            {
                Save();
            }
            else
            {
                Edit();
            }
        }

        public virtual bool HasUnsavedChanges()
        {
            return false;
        }

    }
}
