using Matisco.Core;
using Microsoft.Practices.ServiceLocation;
using Prism.Mvvm;

namespace Example.BusinessApp.Infrastructure.Screens
{
    public class ScreenBase : BindableBase
    {
        private ITranslationService _translationService;

        public ScreenBase(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        protected ITranslationService TranslationService => _translationService;

        protected string Translate(string code)
        {
            return TranslationService.GetTranslation(GetType().FullName + "." + code);
        }
    }
}
