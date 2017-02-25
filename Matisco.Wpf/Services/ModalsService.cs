using System;
using System.Linq;
using Matisco.Wpf.Interfaces;
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

        public void OpenModal(object parent, Action<IResultDataCollection> onWindowClosedAction, string title, string message, ModalIconEnum icons, params ModalButtonEnum[] buttons)
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
            OpenModal(parent, null, title, message, ModalIconEnum.Information, ModalButtonEnum.Ok);
        }

        public void YesNoConfirm(object parent, Action<ModalButtonEnum> resultAction, string title, string message)
        {
            var action = resultAction == null ? null : new Action<IResultDataCollection>((args) => resultAction((ModalButtonEnum) args.GetResults().First().Results[0]));

            OpenModal(parent, action, title, message, ModalIconEnum.Question, ModalButtonEnum.Yes, ModalButtonEnum.No);
        }
    }
}