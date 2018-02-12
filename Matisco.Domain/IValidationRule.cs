using System.Collections.Generic;

namespace Matisco.Core
{
    public interface IValidationRule<T>
    {
        IEnumerable<ValidationError> Validate(T obj);
    }
}
