using System;

namespace Matisco.Core.Specs
{
    public class TranslationSpecs : LocalizedBase
    {
        public TranslationSpecs(Lazy<ITranslationService> translationService) : base(translationService)
        {
        }
    }
}
