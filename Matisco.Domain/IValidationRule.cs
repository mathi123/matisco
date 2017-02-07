using System.Collections.Generic;

namespace Matisco.Domain
{
    public interface IValidationRule<T>
    {
        IEnumerable<ValidationError> Validate(T obj);
    }
}
