using System;
using Example.BusinessApp.Infrastructure.Models;
using Matisco.Domain;

namespace Example.BusinessApp.Sales.Validations
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
