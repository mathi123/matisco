using System;
using System.Collections.Generic;

namespace Matisco.Domain
{
    public abstract class ValidationProviderBase<T> : IValidationProvider<T>
    {
        private readonly Lazy<ITranslationService> _translationService;
        private readonly List<IValidationRule<T>> _rules = new List<IValidationRule<T>>();

        protected ValidationProviderBase(Lazy<ITranslationService> translationService)
        {
            _translationService = translationService;
        }

        public abstract void DefineRules();

        public IEnumerable<IValidationRule<T>> GetRules()
        {
            return _rules;
        }

        protected void Define(ValidationRule<T> rule)
        {
            _rules.Add(rule);   
        }

        protected void Define(Func<T, IEnumerable<ValidationError>> validationFunction)
        {
            _rules.Add(new ValidationRule<T>(validationFunction));
        }

        protected void DefineRequired(string property, Func<T, bool> function)
        {
            _rules.Add(new RequiredPropertyRule<T>(property, function));
        }

        protected void DefineRequired(string property)
        {
            _rules.Add(new RequiredPropertyRule<T>(property, arg => true));   
        }

        public string Translate(string code)
        {
            var id = GetType().FullName + "." + code;
            return _translationService.Value.GetTranslation(id);
        }
    }
}