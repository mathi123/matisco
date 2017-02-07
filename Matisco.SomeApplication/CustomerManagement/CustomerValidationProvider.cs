using System;
using Matisco.Domain;

namespace Matisco.SomeApplication.CustomerManagement
{
    public class CustomerValidationProvider : ValidationProviderBase<Customer>
    {
        public CustomerValidationProvider(Lazy<ITranslationService> translationService) : base(translationService)
        {
        }

        public override void DefineRules()
        {
            DefineRequired(nameof(Customer.Name));
        }
    }
}
