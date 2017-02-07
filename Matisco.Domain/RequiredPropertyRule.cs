using System;
using System.Collections.Generic;

namespace Matisco.Domain
{
    public class RequiredPropertyRule<T> : IValidationRule<T>
    {
        private readonly Func<T, bool> _requiredFunction;

        public string PropertyNameName { get; }

        public RequiredPropertyRule(string propertyName, Func<T, bool> requiredFunction)
        {
            PropertyNameName = propertyName;
            _requiredFunction = requiredFunction;

            if (typeof(T).GetProperty(PropertyNameName) == null)
            {
                throw new ArgumentException($"Objects of type {typeof(T)} do not have a property named {propertyName}");
            }
        }

        protected bool IsRequired(T obj)
        {
            return _requiredFunction(obj);
        }

        public IEnumerable<ValidationError> Validate(T obj)
        {
            if (IsRequired(obj))
            {
                var propertyValue = typeof(T).GetProperty(PropertyNameName).GetValue(obj);

                if(propertyValue == null || (typeof(T) == typeof(string) && (string) propertyValue == ""))
                    yield return new ValidationError($"Field {PropertyNameName} is required!");
            }
        }
    }
}
