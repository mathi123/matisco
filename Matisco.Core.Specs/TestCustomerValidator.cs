using System.Collections.Generic;
using System.Linq;

namespace Matisco.Core.Specs
{
    public class TestCustomerValidator : ICanValidate
    {
        private readonly IValidationProvider<TestCustomer> _provider;
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public TestCustomer Customer { get; set; }

        public TestCustomerValidator(IValidationProvider<TestCustomer> provider)
        {
            _provider = provider;
        }

        public IEnumerable<ValidationError> GetValidationErrors()
        {
            return _errors;
        }

        public bool IsValid()
        {
            _errors.Clear();

            foreach (var validationRule in _provider.GetRules())
            {
                _errors.AddRange(validationRule.Validate(Customer));
            }

            return !_errors.Any();
        }
    }
}
