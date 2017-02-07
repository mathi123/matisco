using System.Collections.Generic;

namespace Matisco.Domain
{
    public interface IValidationProvider<T>
    {
        void DefineRules();
        IEnumerable<IValidationRule<T>> GetRules();
    }
}
