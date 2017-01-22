using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Matisco.Wpf;
using Matisco.Wpf.Interfaces;
using Matisco.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Matisco.SomeApplication
{
    public class ConfirmViewModel : BindableBase, IHasResults
    {
        private readonly IWindowService _windowService;
        private ConfirmResult _result = ConfirmResult.Cancel;

        public ICommand YesCommand => new DelegateCommand(Yes);

        public ICommand NoCommand => new DelegateCommand(No);

        public ConfirmViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void No()
        {
            _result = ConfirmResult.No;
            _windowService.CloseContainingWindow(this);
        }

        private void Yes()
        {
            _result = ConfirmResult.Yes;
            _windowService.CloseContainingWindow(this);
        }

        public object[] GetResults()
        {
            return new object[] { _result };
        }
    }

    public enum ConfirmResult
    {
        Cancel,
        Yes,
        No
    }
}
