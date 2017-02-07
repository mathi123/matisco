using System;
using System.Collections.Generic;

namespace Matisco.Domain.Specs
{
    public class TestCustomerValidationProvider : ValidationProviderBase<TestCustomer>
    {
        public TestCustomerValidationProvider(Lazy<ITranslationService> translationService) : base(translationService)
        {
        }

        public override void DefineRules()
        {
            DefineRequired(nameof(TestCustomer.Name));
            DefineRequired(nameof(TestCustomer.City), customer => !string.IsNullOrEmpty(customer.Country));
            Define(ValidateEmail);
        }

        private IEnumerable<ValidationError> ValidateEmail(TestCustomer arg)
        {
            if (arg.Email != null && !arg.Email.Contains("@"))
            {
                yield return new ValidationError("Invalid email");
            }
        }

    }
}
