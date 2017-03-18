using System;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Models;

namespace Matisco.Wpf.Services
{
    public interface IModalsService
    {
        void OpenModal(object parent, Action<IResultDataCollection> onWindowClosedAction, string title, string message, ModalIconEnum icons, params ModalButtonEnum[] buttons);

        void InfoMessage(object parent, string title, string message);

        void WarningMessage(object parent, string title, string message);

        void ErrorMessage(object parent, string title, string message);

        void YesNoConfirm(object parent, Action<ModalButtonEnum> resultAction, string title, string message);
    }
}
