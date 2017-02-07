using System;

namespace Matisco.Domain.Specs
{
    public class TranslationSpecs : LocalizedBase
    {
        public TranslationSpecs(Lazy<ITranslationService> translationService) : base(translationService)
        {
        }
    }
}
