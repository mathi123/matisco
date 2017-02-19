using System;
using Matisco.Wpf.Models;
using Matisco.Wpf.ViewModels;
using Matisco.Wpf.Views;
using Prism.Regions;

namespace Matisco.Wpf.Services
{
    public class ModalsService : IModalsService
    {
        private readonly IWindowService _windowService;

        public ModalsService(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void OpenModal(object parent, Action<object[]> onWindowClosedAction, string title, string message, ModalButtonEnum buttons,
            ModalIconEnum icons)
        {
            var navigationParameters = new NavigationParameters
            {
                {nameof(ModalViewModel.Title), title},
                {nameof(ModalViewModel.Message), message},
                {nameof(ModalButtonEnum), buttons},
                {nameof(ModalIconEnum), icons}
            };

            _windowService.OpenDialog(parent, nameof(ModalView), navigationParameters, onWindowClosedAction);
        }

        public void InfoMessage(object parent, string title, string message)
        {
            OpenModal(parent, null, title, message, ModalButtonEnum.Ok, ModalIconEnum.Information);
        }

        public void YesNoConfirm(object parent, Action<ModalButtonEnum> resultAction, string title, string message)
        {
            var action = new Action<object[]>((args) => resultAction((ModalButtonEnum) args[0]));
            var buttons = ModalButtonEnum.Yes | ModalButtonEnum.No;

            OpenModal(parent, action, title, message, buttons, ModalIconEnum.Question);
        }
    }
}