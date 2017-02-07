using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matisco.Domain.Specs
{
    [TestClass]
    public class LocalizationSpecs
    {
        [TestMethod]
        public void TestTranslation()
        {
            var lazy = new Lazy<ITranslationService>(CreateService);
            var localizedEntity = new TranslationSpecs(lazy);

            Assert.AreEqual("Een beetje text", localizedEntity.Translate("SomeText"));
            Assert.AreEqual("[SomeMissingText]", localizedEntity.Translate("SomeMissingText"));
        }

        private ITranslationService CreateService()
        {
            var translationService = new TranslationService();
            translationService.SetDefaultLanguageCode("nl");
            return translationService;
        }
    }
}
