using System;

namespace Matisco.Domain
{
    public abstract class LocalizedBase : ILocalized
    {
        private readonly Lazy<ITranslationService> _translationService;

        protected LocalizedBase(Lazy<ITranslationService> translationService)
        {
            _translationService = translationService;
        }

        public string Translate(string code)
        {
            return _translationService.Value.GetTranslation($"{GetType().FullName}.{code}");
        }
    }
}
