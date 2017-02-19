using System;
using Matisco.Wpf.Models;

namespace Matisco.Wpf.Services
{
    public interface IModalsService
    {
        void OpenModal(object parent,  Action<object[]> onWindowClosedAction, string title, string message, ModalButtonEnum buttons, ModalIconEnum icons);

        void InfoMessage(object parent, string title, string message);

        void YesNoConfirm(object parent, Action<ModalButtonEnum> resultAction, string title, string message);
    }
}
