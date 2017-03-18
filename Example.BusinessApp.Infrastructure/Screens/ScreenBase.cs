using Matisco.Domain;
using Microsoft.Practices.ServiceLocation;
using Prism.Mvvm;

namespace Example.BusinessApp.Infrastructure.Screens
{
    public class ScreenBase : BindableBase
    {
        private ITranslationService _translationService;

        protected ITranslationService TranslationService
        {
            get
            {
                if (_translationService == null)
                {
                    _translationService = ServiceLocator.Current.GetInstance<ITranslationService>();
                }
                return _translationService;
            }
        }

        protected string Translate(string code)
        {
            return TranslationService.GetTranslation(GetType().FullName + "." + code);
        }
    }
}
