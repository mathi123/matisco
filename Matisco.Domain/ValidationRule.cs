using System;
using System.Collections.Generic;

namespace Matisco.Domain
{
    public class ValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, IEnumerable<ValidationError>> _validationFunction;

        public ValidationRule(Func<T, IEnumerable<ValidationError>> validationFunction)
        {
            _validationFunction = validationFunction;
        }

        public IEnumerable<ValidationError> Validate(T obj)
        {
            return _validationFunction(obj);
        }
    }
}