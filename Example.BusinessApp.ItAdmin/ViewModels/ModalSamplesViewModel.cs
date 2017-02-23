using System;
using System.Windows.Input;
using Matisco.Wpf.Models;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Example.BusinessApp.ItAdmin.ViewModels
{
    public class ModalSamplesViewModel : BindableBase
    {
        private readonly IModalsService _modalService;
        private readonly IExceptionHandler _exceptionHandler;

        public ICommand OpenInfoModalCommand => new DelegateCommand(OpenInfoModal);
        public ICommand OpenWarningModalCommand => new DelegateCommand(OpenWarningModal);
        public ICommand OpenErrorModalCommand => new DelegateCommand(OpenErrorModal);
        public ICommand OpenEmptyModalCommand => new DelegateCommand(OpenEmptyModal);
        public ICommand OpenQuestionModalCommand => new DelegateCommand(OpenQuestionModal);
        public ICommand ThrowExceptionCommand => new DelegateCommand(ThrowException);
        public ICommand ThrowSmallExceptionCommand => new DelegateCommand(ThrowSmallException);


        public ModalSamplesViewModel(IModalsService modalService, IExceptionHandler exceptionHandler)
        {
            _modalService = modalService;
            _exceptionHandler = exceptionHandler;
        }

        private void OpenWarningModal()
        {
            _modalService.OpenModal(this, null, "Warning", "This is an warning modal.", ModalIconEnum.Warning, ModalButtonEnum.Ok);
        }

        private void OpenErrorModal()
        {
            _modalService.OpenModal(this, null, "Error", "This is an error modal.", ModalIconEnum.Error, ModalButtonEnum.Ok);
        }

        private void OpenEmptyModal()
        {
            _modalService.OpenModal(this, null, "Empty", "This is an empty modal.", ModalIconEnum.None, ModalButtonEnum.Ok, ModalButtonEnum.Cancel);
        }

        private void OpenQuestionModal()
        {
            _modalService.YesNoConfirm(this, null, "Question", "Do you want to answer this question?");
        }

        private void OpenInfoModal()
        {
            _modalService.InfoMessage(this, "Info", "This is an information modal.");
        }

        private void ThrowException()
        {
            var exception = new ArgumentException("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel orem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel dignissim nunc. Donec iaculis enim tellus, at pretium mauris ultrices eu. Sed eros est, euismod et viverra eget, pellentesque nec diam. Vestibulum elementum tellus in lacus pellentesque, quis ullamcorper metus eleifend. Nullam id vestibulum elit. Nunc lacinia eleifend accumsan. Phasellus in neque purus. Sed hendrerit sit amet lacus eget luctus. Sed eget placerat augue. Duis aliquet purus purus, vitae vestibulum ligula euismod nec.");
            _exceptionHandler.Handle(this, exception, "Some error ocurred");
        }

        private void ThrowSmallException()
        {
            var exception = new ArgumentException("Missing projectId;");

            _exceptionHandler.Handle(this, exception);
        }
    }
}