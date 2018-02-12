using System.Collections.Generic;

namespace Matisco.Core
{
    public interface IValidationProvider<T>
    {
        void DefineRules();
        IEnumerable<IValidationRule<T>> GetRules();
    }
}
